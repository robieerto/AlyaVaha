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
  DxTotalItem
} from 'devextreme-vue/data-grid'
import DxButton from 'devextreme-vue/button'
import { DxLoadPanel } from 'devextreme-vue/load-panel'
import { reactive } from 'vue'

import { exportDataGrid } from 'devextreme/excel_exporter'
import { Workbook } from 'exceljs'
import saveAs from 'file-saver'

import { dateFormat, timeFormat, floatFormat, filterOperations } from '@/utils/helpers'
import store from '@/store'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  dataGridInstance: null
})

function getNavazovania() {
  sendCommand('GetNavazovania')
}

getNavazovania()

function onDataGridInitialized(e) {
  state.dataGridInstance = e.component
}

function addRow(rowEvent) {
  sendCommand('AddNavazovanie', rowEvent.data)
}

function updateRow(rowEvent) {
  sendCommand('UpdateNavazovanie', { ...rowEvent.oldData, ...rowEvent.newData })
}

function deleteRow(rowEvent) {
  sendCommand('DeleteNavazovanie', rowEvent.data.Id)
}

const calculatePoradie = (rowIndex) =>
  rowIndex + state.dataGridInstance.pageIndex() * state.dataGridInstance.pageSize()

function calculateTimeCellValue(rowData) {
  return rowData.DatumKonca.toString()
}

function calculateTimeFilterExpression(filterValue, selectedFilterOperation) {
  const getTime = (date) => new Date(date).toLocaleTimeString('sk-SK').slice(0, -3)

  // Override implementation for the "between" filter operation
  if (selectedFilterOperation === 'between' && Array.isArray(filterValue)) {
    const filterExpression = [
      [this.dataField, '>', getTime(filterValue[0])],
      'and',
      [this.dataField, '<', getTime(filterValue[1])]
    ]
    return filterExpression
  }
  // Invoke the default implementation for other filter operations
  return [this.dataField, selectedFilterOperation, getTime(filterValue)]
}

function exportToXls() {
  console.log(state.dataGridInstance?.getCombinedFilter())

  if (state.zaznamyCount > 50000) {
    alert('Nie je možné exportovať viac ako 50 000 záznamov')
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
      saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'BigBagData.xlsx')
    })
  })
}
</script>

<template>
  <div>
    <div class="d-flex justify-content-between">
      <div class="d-flex justify-content-start">
        <h2 class="content-block">Prehľad navažovaní</h2>
        <DxButton icon="refresh" class="mt-3" @click="getNavazovania"> </DxButton>
      </div>
      <div>
        <button type="button" class="btn btn-primary h-50 m-3 mr-0" @click="exportToXls">
          Exportovať do súboru
        </button>
        <!-- <button type="button" class="btn btn-secondary h-50 m-3 ml-0" @click="exportToPdf">Exportovať do PDF</button> -->
      </div>
    </div>
    <DxLoadPanel v-model:visible="store.navazovaniaLoading" :position="{ of: '#data-grid' }" />
    <DxDataGrid
      id="data-grid"
      class="dx-card wide-card"
      :data-source="store.navazovania"
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
      @initialized="onDataGridInitialized"
      @row-inserting="addRow"
      @row-updating="updateRow"
      @row-removing="deleteRow"
    >
      <DxPaging :page-size="10" />
      <DxPager :show-page-size-selector="true" :show-info="true" />
      <DxFilterRow :visible="true" />
      <DxEditing :allow-updating="false" :allow-deleting="true" :allow-adding="false" mode="row" />
      <DxExport :enabled="false"></DxExport>
      <!-- <dx-column caption="Riadok" :allow-search="false" :allow-sorting="false" :alignment="'right'" cell-template="poradieTemplate" /> -->
      <DxColumn data-field="Id" caption="Id" width="100" :visible="false" />
      <DxColumn
        caption="Riadok"
        :visible="false"
        :allow-search="false"
        :allow-sorting="false"
        :allow-exporting="true"
        :alignment="'right'"
        cell-template="poradieTemplate"
      />
      <DxColumn
        data-field="DatumStartu"
        caption="Dátum navažovania"
        data-type="date"
        :min-width="200"
        :format="dateFormat"
        :allow-editing="false"
        :filterOperations="filterOperations"
      />
      <!-- <DxColumn
        data-field="DatumKonca"
        caption="Dátum konca"
        data-type="datetime"
        width="140"
        :format="timeFormat"
        :allow-editing="false"
        :editorOptions="{ type: 'time' }"
        :calculate-cell-value="calculateTimeCellValue"
        :calculate-filter-expression="calculateTimeFilterExpression"
      /> -->
      <DxColumn data-field="ZariadenieId" caption="Zariadenie" :min-width="150">
        <DxLookup :data-source="store.zariadenia" value-expr="Id" display-expr="NazovZariadenia" />
      </DxColumn>
      <DxColumn
        data-field="NavazeneMnozstvo"
        caption="Navážené množstvo"
        data-type="number"
        :min-width="100"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="NavazenyPocetDavok"
        caption="Navážený počet dávok"
        data-type="number"
        :min-width="100"
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
      <DxColumn data-field="OdkialId" caption="Zásobník odkiaľ" :min-width="180">
        <DxLookup :data-source="store.zasobniky" value-expr="Id" display-expr="NazovZasobnika" />
      </DxColumn>
      <DxColumn data-field="KamId" caption="Zásobník kam" :min-width="180">
        <DxLookup :data-source="store.zasobniky" value-expr="Id" display-expr="NazovZasobnika" />
      </DxColumn>
      <DxSummary>
        <DxTotalItem column="Riadok" summary-type="count" />
        <DxTotalItem column="NavazeneMnozstvo" summary-type="sum" :value-format="floatFormat" />
      </DxSummary>
      <template #poradieTemplate="{ data }">{{ calculatePoradie(data.row.rowIndex) }}</template>
    </DxDataGrid>
  </div>
</template>
