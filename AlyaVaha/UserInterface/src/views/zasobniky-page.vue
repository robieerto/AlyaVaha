<script setup>
import 'devextreme/data/odata/store'
import DxDataGrid, {
  DxColumn,
  DxFilterRow,
  DxPager,
  DxPaging,
  DxEditing
} from 'devextreme-vue/data-grid'
import { DxLoadPanel } from 'devextreme-vue/load-panel'
import { DxRequiredRule } from 'devextreme-vue/validator'
import { reactive, watch } from 'vue'

import { dateTimeFormat, filterOperations, filterStringOperations } from '@/utils/helpers'
import store from '@/store'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  dataGridInstance: null
})

watch(
  () => store.zasobniky,
  () => {
    state.dataGridInstance.refresh()
  }
)

function onDataGridInitialized(e) {
  state.dataGridInstance = e.component
}

function initNewRow(rowEvent) {
  rowEvent.data.CestaDoVahy = true
  rowEvent.data.CestaZVahy = true
}

function addRow(rowEvent) {
  sendCommand('AddZasobnik', rowEvent.data)
}

function updateRow(rowEvent) {
  sendCommand('UpdateZasobnik', { ...rowEvent.oldData, ...rowEvent.newData })
}

function deleteRow(rowEvent) {
  sendCommand('DeleteZasobnik', rowEvent.data.Id)
}

// const calculatePoradie = (rowIndex) =>
//   rowIndex + state.dataGridInstance.pageIndex() * state.dataGridInstance.pageSize()
</script>

<template>
  <div>
    <h2 class="content-block">Evidencia zásobníkov</h2>
    <DxLoadPanel v-model:visible="store.zasobnikyLoading" :position="{ of: '#data-grid' }" />
    <DxDataGrid
      id="data-grid"
      class="dx-card wide-card"
      :data-source="store.zasobniky"
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
      @init-new-row="initNewRow"
      @row-inserting="addRow"
      @row-updating="updateRow"
      @row-removing="deleteRow"
    >
      <DxPaging :page-size="10" />
      <DxPager :show-page-size-selector="true" :show-info="true" />
      <DxFilterRow :visible="true" />
      <DxEditing
        :allow-updating="store.isUserAdmin"
        :allow-deleting="store.isUserAdmin"
        :allow-adding="store.isUserAdmin"
        mode="row"
      />
      <!-- <dx-column caption="Riadok" :allow-search="false" :allow-sorting="false" :alignment="'right'" cell-template="poradieTemplate" /> -->
      <DxColumn data-field="Id" caption="Id" :width="100" :visible="false" />
      <DxColumn
        data-field="NazovZasobnika"
        caption="Názov zásobníka"
        :filterOperations="filterStringOperations"
      >
        <DxRequiredRule />
      </DxColumn>
      <DxColumn
        data-field="DatumVytvorenia"
        caption="Dátum vytvorenia"
        data-type="date"
        :format="dateTimeFormat"
        :allow-editing="false"
        :filterOperations="filterOperations"
      />
      <DxColumn
        data-field="DatumUpravy"
        caption="Dátum úpravy"
        data-type="date"
        :format="dateTimeFormat"
        :allow-editing="false"
        :filterOperations="filterOperations"
      />
      <DxColumn data-field="CestaDoVahy" data-type="boolean" caption="Cesta do váhy" :width="160" />
      <DxColumn data-field="CestaZVahy" data-type="boolean" caption="Cesta z váhy" :width="160" />
      <DxColumn data-field="Skratka" caption="Skratka" :filterOperations="filterStringOperations" />
      <!-- <template #poradieTemplate="{ data }">{{ calculatePoradie(data.row.rowIndex) }}</template> -->
    </DxDataGrid>
  </div>
</template>
