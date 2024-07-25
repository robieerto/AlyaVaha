<script setup>
import 'devextreme/data/odata/store'
import DxDataGrid, {
  DxColumn,
  DxFilterRow,
  DxPager,
  DxPaging,
  DxEditing,
  DxLookup,
  DxExport,
  DxSummary,
  DxTotalItem,
  DxHeaderFilter
} from 'devextreme-vue/data-grid'
import DxButton from 'devextreme-vue/button'
import CustomStore from 'devextreme/data/custom_store'
import { reactive, watch } from 'vue'
import { exportDataGrid } from 'devextreme/excel_exporter'
import { Workbook } from 'exceljs'
import saveAs from 'file-saver'

import { dateFormat, timeFormat, notify, filterOperations } from '@/utils/helpers'
import store from '@/store'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  dataGridInstance: null,
  dataSource: new CustomStore({
    key: 'Id',
    load: function (loadOptions) {
      return new Promise((resolve, reject) => {
        if (loadOptions.group?.length) {
          switch (loadOptions.group[0].selector) {
            case 'ZariadenieId':
              resolve({
                data: store.zariadenia.map((e) => ({ key: e.Id }))
              })
              break
            case 'MaterialId':
              resolve({
                data: store.materialy.map((e) => ({ key: e.Id }))
              })
              break
            case 'OdkialId':
              resolve({
                data: store.zasobniky
                  .filter((item) => item.CestaDoVahy === true)
                  .map((e) => ({ key: e.Id }))
              })
              break
            case 'KamId':
              resolve({
                data: store.zasobniky
                  .filter((item) => item.CestaZVahy === true)
                  .map((e) => ({ key: e.Id }))
              })
              break
            default:
              resolve({ data: [] })
          }
        }
        if (!store.navazovaniaData) {
          return resolve({ data: [], totalCount: 0 })
        }
        try {
          // Simulate server-side processing
          if (!loadOptions.isLoadingAll) {
            resolve(store.navazovaniaData)
          } else {
            resolve(store.navazovaniaDataExport)
          }
        } catch (error) {
          reject(error)
        }
      })
    },
    remove: function (key) {
      return sendCommand('DeleteNavazovanie', key)
    }
  })
})

watch(
  () => store.navazovaniaData,
  () => {
    state.dataGridInstance.refresh()
  }
)

function onDataGridInitialized(e) {
  state.dataGridInstance = e.component
  getActualData(e)
}

const calculatePoradie = (rowIndex) =>
  rowIndex + state.dataGridInstance.pageIndex() * state.dataGridInstance.pageSize()

function calculateTimeCellValue(rowData) {
  return rowData.CasStartu.toString()
}

function calculateTimeFilterExpression(filterValue, selectedFilterOperation) {
  const getTime = (date) =>
    date
      ? new Intl.DateTimeFormat('en-US', {
          hour: 'numeric',
          minute: 'numeric',
          hour12: false
        }).format(new Date(date))
      : null

  // Override implementation for the "between" filter operation
  if (selectedFilterOperation === 'between' && Array.isArray(filterValue)) {
    console.log(filterValue)
    const filterExpression = [
      [this.dataField, '>=', getTime(filterValue[0])],
      'and',
      [this.dataField, '<=', getTime(filterValue[1])]
    ]

    console.log(filterExpression)
    return filterExpression
  }
  // Invoke the default implementation for other filter operations
  return [this.dataField, selectedFilterOperation, getTime(filterValue)]
}

function getNavazovania(dataSourceLoadOptions) {
  sendCommand('GetNavazovania', dataSourceLoadOptions)
}

function getActualData(e, allData = false) {
  const component = e.component
  const dataSourceLoadOptions = {}
  if (!allData) {
    dataSourceLoadOptions.Skip = component.pageIndex() * component.pageSize()
    dataSourceLoadOptions.Take = component.pageSize()
  }
  dataSourceLoadOptions.Filter = component.getCombinedFilter()
  dataSourceLoadOptions.Sort = []
  for (let i = 0; i < component.columnCount(); i++) {
    if (component.columnOption(i).sortOrder === 'asc') {
      dataSourceLoadOptions.Sort.push({
        Selector: component.columnOption(i).dataField,
        Desc: false
      })
    }
  }
  dataSourceLoadOptions.TotalSummary = [
    { Selector: 'DatumStartu', SummaryType: 'count' },
    { Selector: 'NavazeneMnozstvo', SummaryType: 'sum' },
    { Selector: 'NavazenyPocetDavok', SummaryType: 'sum' }
  ]
  dataSourceLoadOptions.RequireTotalCount = true
  getNavazovania(dataSourceLoadOptions)
}

function onOptionChanged(e) {
  if (e.name && (e.name.startsWith('focused') || e.name === 'dataSource')) return
  var allData = e.name === 'loadPanel'
  getActualData(e, allData)
}

function exportToXls() {
  console.log(store.navazovaniaData.totalCount)
  if (store.navazovaniaData.totalCount > 4) {
    notify('Nie je možné exportovať viac ako 50 000 záznamov', 'error')
    return
  }
  const workbook = new Workbook()
  const worksheet = workbook.addWorksheet('Harok1')
  exportDataGrid({
    component: state.dataGridInstance,
    worksheet: worksheet,
    customizeCell: function (options) {
      options.excelCell.font = { name: 'Arial', size: 12 }
      options.excelCell.alignment = { horizontal: 'left' }

      if (options.gridCell.rowType === 'data' && options.gridCell.column.caption === 'Riadok') {
        options.excelCell.value = options.excelCell.row - 1
      }
    }
  }).then(function () {
    workbook.xlsx.writeBuffer().then(function (buffer) {
      saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'AlyaVahaData.xlsx')
    })
  })
}
</script>

<template>
  <div>
    <div class="d-flex justify-content-between">
      <div class="d-flex justify-content-start">
        <h2 class="content-block">Prehľad navažovaní</h2>
        <DxButton
          icon="refresh"
          class="mt-3"
          @click="() => getActualData({ component: state.dataGridInstance })"
        >
        </DxButton>
      </div>
      <div>
        <button type="button" class="btn btn-primary h-50 m-3 mr-0" @click="exportToXls">
          Exportovať do súboru
        </button>
        <!-- <button type="button" class="btn btn-secondary h-50 m-3 ml-0" @click="exportToPdf">Exportovať do PDF</button> -->
      </div>
    </div>
    <!-- <DxLoadPanel v-model:visible="store.navazovaniaLoading" :position="{ of: '#data-grid' }" /> -->
    <DxDataGrid
      id="data-grid"
      class="dx-card wide-card"
      :data-source="state.dataSource"
      :key-expr="'Id'"
      :width="'100%'"
      :show-borders="false"
      :focused-row-index="0"
      :focused-row-enabled="true"
      :auto-navigate-to-focused-row="false"
      :column-auto-width="true"
      :show-column-lines="true"
      :show-row-lines="true"
      :word-wrap-enabled="true"
      :remote-operations="true"
      @initialized="onDataGridInitialized"
      @option-changed="onOptionChanged"
    >
      <DxPaging :page-size="10" />
      <DxPager :show-page-size-selector="false" :show-info="true" />
      <DxFilterRow :visible="true" />
      <DxEditing :allow-updating="false" :allow-deleting="true" :allow-adding="false" mode="row" />
      <DxExport :enabled="false"></DxExport>
      <!-- <dx-column caption="Riadok" :allow-search="false" :allow-sorting="false" :alignment="'right'" cell-template="poradieTemplate" /> -->
      <DxColumn data-field="Id" caption="Id" width="100" :visible="false" />
      <!-- <DxColumn
        caption="Riadok"
        :visible="false"
        :allow-search="false"
        :allow-sorting="false"
        :allow-exporting="true"
        :alignment="'right'"
        cell-template="poradieTemplate"
      /> -->
      <DxColumn
        data-field="DatumStartu"
        caption="Dátum navažovania"
        data-type="date"
        :min-width="170"
        :format="dateFormat"
        :allow-editing="false"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="CasStartu"
        caption="Čas navažovania"
        data-type="datetime"
        width="140"
        :format="timeFormat"
        :allow-editing="false"
        :editorOptions="{ type: 'time' }"
        :calculate-cell-value="calculateTimeCellValue"
        :calculate-filter-expression="calculateTimeFilterExpression"
        :filterOperations="filterOperations"
      />
      <DxColumn data-field="ZariadenieId" caption="Zariadenie" :min-width="150">
        <DxHeaderFilter :data-source="store.zariadenia" />
        <DxLookup :data-source="store.zariadenia" value-expr="Id" display-expr="NazovZariadenia" />
      </DxColumn>
      <DxColumn
        data-field="NavazeneMnozstvo"
        caption="Navážené množstvo"
        data-type="number"
        :min-width="180"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="NavazenyPocetDavok"
        caption="Navážený počet dávok"
        data-type="number"
        :min-width="180"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="PozadovaneMnozstvo"
        caption="Požadované množstvo"
        data-type="number"
        :min-width="100"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="PozadovanyPocetDavok"
        caption="Požadovaný počet dávok"
        data-type="number"
        :min-width="100"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="VelkostDavky"
        caption="Veľkosť dávky"
        data-type="number"
        :min-width="100"
        :filterOperations="filterOperations"
      />
      <DxColumn data-field="MaterialId" caption="Materiál" :min-width="150">
        <DxLookup :data-source="store.materialy" value-expr="Id" display-expr="NazovMaterialu" />
      </DxColumn>
      <DxColumn data-field="OdkialId" caption="Zásobník odkiaľ" :min-width="180">
        <DxLookup :data-source="store.zasobniky" value-expr="Id" display-expr="NazovZasobnika" />
      </DxColumn>
      <DxColumn data-field="KamId" caption="Zásobník kam" :min-width="180">
        <DxLookup :data-source="store.zasobniky" value-expr="Id" display-expr="NazovZasobnika" />
      </DxColumn>
      <DxSummary>
        <DxTotalItem column="DatumStartu" summary-type="count" :display-format="'Spolu: {0}'" />
        <DxTotalItem column="NavazeneMnozstvo" summary-type="sum" :display-format="'{0}'" />
        <DxTotalItem column="NavazenyPocetDavok" summary-type="sum" :display-format="'{0}'" />
      </DxSummary>
      <template #poradieTemplate="{ data }">{{ calculatePoradie(data.row.rowIndex) }}</template>
    </DxDataGrid>
  </div>
</template>
