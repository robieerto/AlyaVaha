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
  // DxSummary,
  // DxTotalItem,
  DxHeaderFilter,
  DxSorting
} from 'devextreme-vue/data-grid'
import { DxPopup } from 'devextreme-vue'
import DxButton from 'devextreme-vue/button'
import { DxCheckBox } from 'devextreme-vue/check-box'
import { DxLoadPanel } from 'devextreme-vue/load-panel'
import CustomStore from 'devextreme/data/custom_store'
import { reactive, watch } from 'vue'
import { exportDataGrid } from 'devextreme/excel_exporter'
import { Workbook } from 'exceljs'
import saveAs from 'file-saver'

import { dateFormat, timeFormat, dateTimeFormat, notify, filterOperations } from '@/utils/helpers'
import store from '@/store'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  dataGridInstance: null,
  confirmDialogVisible: false,
  dataSource: new CustomStore({
    key: 'Id',
    load: function (loadOptions) {
      return new Promise((resolve, reject) => {
        // Load options for row filtering selectors
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
            case 'UzivatelId':
              resolve({
                data: store.uzivatelia.map((e) => ({ key: e.Id }))
              })
              break
            default:
              resolve({ data: [] })
          }
        }

        try {
          // Simulate server-side processing
          if (!loadOptions.isLoadingAll) {
            getNavazovania(loadOptions)
            watch(
              () => store.navazovaniaData,
              () => {
                if (store.navazovaniaData) {
                  store.navazovaniaLoading = false
                  resolve(store.navazovaniaData)
                }
              }
            )
          } else {
            getExportNavazovania(loadOptions)
            watch(
              () => store.navazovaniaDataExport,
              () => {
                if (store.navazovaniaDataExport) {
                  store.navazovaniaLoading = false
                  resolve(store.navazovaniaDataExport)
                }
              }
            )
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
  () => store.nastavenia.DatumACasV1Stlpci,
  () => {
    state.dataGridInstance.clearFilter()
    sendCommand('UpdateNastavenia', store.nastavenia)
    // state.dataGridInstance.repaint()
  }
)

watch(
  () => store.isNavazovaniaDeleting,
  () => {
    if (!store.isNavazovaniaDeleting) {
      state.dataGridInstance.refresh()
    }
  }
)

function onDataGridInitialized(e) {
  state.dataGridInstance = e.component
}

const calculatePoradie = (rowIndex) =>
  rowIndex + state.dataGridInstance.pageIndex() * state.dataGridInstance.pageSize()

function calculateStartTimeCellValue(rowData) {
  return rowData.CasStartu?.toString()
}

function calculateEndTimeCellValue(rowData) {
  return rowData.CasKonca?.toString()
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
    const filterExpression = [
      [this.dataField, '>=', getTime(filterValue[0])],
      'and',
      [this.dataField, '<=', getTime(filterValue[1])]
    ]

    return filterExpression
  }
  // Invoke the default implementation for other filter operations
  return [this.dataField, selectedFilterOperation, getTime(filterValue)]
}

function getNavazovania(dataSourceLoadOptions) {
  store.navazovaniaLoading = true
  sendCommand('GetNavazovania', dataSourceLoadOptions)
}

function getExportNavazovania(dataSourceLoadOptions) {
  store.navazovaniaLoading = true
  sendCommand('GetExportNavazovania', dataSourceLoadOptions)
}

async function exportToXls() {
  if (state.dataGridInstance?.totalCount?.() > 100000) {
    notify('Nie je možné exportovať viac ako 100 000 záznamov', 'error')
    return
  }
  store.navazovaniaDataExport = null
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

async function deleteByFilter() {
  store.isNavazovaniaDeleting = true
  const dataSourceLoadOptions = {}
  dataSourceLoadOptions.Filter = state.dataGridInstance.getCombinedFilter()
  dataSourceLoadOptions.Skip = 0
  dataSourceLoadOptions.Take = 0
  sendCommand('DeleteNavazovaniaByFilter', dataSourceLoadOptions)
}
</script>

<template>
  <div id="navazovania-page">
    <DxLoadPanel
      :position="{ of: '#navazovania-page' }"
      v-model:visible="store.isNavazovaniaDeleting"
      :show-indicator="true"
      :show-pane="true"
      :shading="true"
      :hide-on-outside-click="false"
      shading-color="rgba(0,0,0,0.4)"
    />
    <div class="d-flex justify-content-between">
      <div class="d-flex justify-content-start">
        <h2 class="content-block">Prehľad navažovaní</h2>
        <DxButton
          icon="refresh"
          class="mt-3 mr-5"
          @click="
            () => {
              store.navazovaniaLoading = true
              state.dataGridInstance.refresh()
            }
          "
        >
        </DxButton>
        <DxCheckBox
          text="Dátum a čas v 1 stĺpci"
          class="ml-5"
          v-model:value="store.nastavenia.DatumACasV1Stlpci"
        />
      </div>
      <div class="d-flex justify-content-start">
        <button
          v-if="store.isUserAdmin"
          type="button "
          class="btn btn-danger h-50 m-3 mr-0"
          @click="() => (state.confirmDialogVisible = true)"
          :disabled="store.isNavazovaniaDeleting"
        >
          Vymazať vyfiltrované
        </button>
        <button
          type="button "
          class="btn btn-primary h-50 m-3 ml-0 mr-0"
          @click="exportToXls"
          :disabled="store.navazovaniaLoading"
        >
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
    >
      <DxPaging :page-size="10" />
      <DxPager :show-page-size-selector="false" :show-info="true" />
      <DxFilterRow :visible="true" />
      <DxEditing
        :allow-updating="false"
        :allow-deleting="store.isUserAdmin"
        :allow-adding="false"
        mode="row"
      />
      <DxExport :enabled="false"></DxExport>
      <DxSorting mode="none" />
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
        caption="Dátum štart navažovania"
        :data-type="store.nastavenia.DatumACasV1Stlpci ? 'datetime' : 'date'"
        :min-width="150"
        :format="store.nastavenia.DatumACasV1Stlpci ? dateTimeFormat : dateFormat"
        :allow-editing="false"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="CasStartu"
        caption="Čas štart navažovania"
        data-type="datetime"
        width="130"
        :visible="!store.nastavenia.DatumACasV1Stlpci"
        :format="timeFormat"
        :allow-editing="false"
        :editorOptions="{ type: 'time' }"
        :calculate-cell-value="calculateStartTimeCellValue"
        :calculate-filter-expression="calculateTimeFilterExpression"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="DatumKonca"
        caption="Dátum koniec navažovania"
        :data-type="store.nastavenia.DatumACasV1Stlpci ? 'datetime' : 'date'"
        :min-width="150"
        :format="store.nastavenia.DatumACasV1Stlpci ? dateTimeFormat : dateFormat"
        :allow-editing="false"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="CasKonca"
        caption="Čas koniec navažovania"
        data-type="datetime"
        width="130"
        :visible="!store.nastavenia.DatumACasV1Stlpci"
        :format="timeFormat"
        :allow-editing="false"
        :editorOptions="{ type: 'time' }"
        :calculate-cell-value="calculateEndTimeCellValue"
        :calculate-filter-expression="calculateTimeFilterExpression"
        :filterOperations="filterOperations"
      />
      <DxColumn data-field="ZariadenieId" caption="Zariadenie" :min-width="150">
        <DxHeaderFilter :data-source="store.zariadenia" />
        <DxLookup :data-source="store.zariadenia" value-expr="Id" display-expr="NazovZariadenia" />
      </DxColumn>
      <DxColumn
        data-field="NavazeneMnozstvo"
        caption="Navážená hmotnosť (kg)"
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
        caption="Požadovaná hmotnosť (kg)"
        data-type="number"
        :min-width="110"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="PozadovanyPocetDavok"
        caption="Požadovaný počet dávok"
        data-type="number"
        :min-width="110"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="VelkostDavky"
        caption="Veľkosť dávky (kg)"
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
      <DxColumn data-field="UzivatelId" caption="Užívateľ" :min-width="180">
        <DxLookup :data-source="store.uzivatelia" value-expr="Id" display-expr="Login" />
      </DxColumn>
      <!-- <DxSummary>
        <DxTotalItem column="DatumStartu" summary-type="count" :display-format="'Spolu: {0}'" />
        <DxTotalItem column="NavazeneMnozstvo" summary-type="sum" :display-format="'{0}'" />
        <DxTotalItem column="NavazenyPocetDavok" summary-type="sum" :display-format="'{0}'" />
      </DxSummary> -->
      <template #poradieTemplate="{ data }">{{ calculatePoradie(data.row.rowIndex) }}</template>
    </DxDataGrid>
  </div>
  <DxPopup
    :visible="store.isUserAdmin && state.confirmDialogVisible"
    title="Potvrdenie zmazania"
    :width="340"
    :height="290"
    :drag-enabled="false"
    :close-on-outside-click="true"
    :showTitle="true"
    :showCloseButton="true"
    :onHidden="() => (state.confirmDialogVisible = false)"
  >
    <div class="my-2 fw-bold">Počet záznamov: {{ state.dataGridInstance?.totalCount?.() }}</div>
    <div class="my-2">Naozaj si prajete vymazať vyfiltrované navažovania?</div>
    <div class="my-2">Uistite sa, že dáta máte prípadne zálohované.</div>
    <div class="my-2">Pri veľkom počte záznamov môže mazanie trvať aj niekoľko minút.</div>

    <!-- Action Buttons -->
    <div class="row justify-content-between mt-3">
      <div class="col-auto">
        <DxButton text="Zrušiť" @click="() => (state.confirmDialogVisible = false)" />
      </div>
      <div class="col-auto">
        <DxButton text="Zmazať" type="danger" @click="deleteByFilter" />
      </div>
    </div>
  </DxPopup>
</template>
