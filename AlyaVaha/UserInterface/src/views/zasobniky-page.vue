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
import { reactive } from 'vue'

import { dateFormat, actualDate } from '@/utils/helpers'
import store from '@/store'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  dataGridInstance: null
})

function onDataGridInitialized(e) {
  state.dataGridInstance = e.component
}

function addRow(rowEvent) {
  sendCommand('AddZasobnik', rowEvent.data)
}

function updateRow(rowEvent) {
  console.log(rowEvent)
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
      @row-inserting="addRow"
      @row-updating="updateRow"
      @row-removing="deleteRow"
    >
      <DxPaging :page-size="10" />
      <DxPager :show-page-size-selector="true" :show-info="true" />
      <DxFilterRow :visible="true" />
      <DxEditing :allow-updating="true" :allow-deleting="true" :allow-adding="true" mode="row" />
      <!-- <dx-column caption="Riadok" :allow-search="false" :allow-sorting="false" :alignment="'right'" cell-template="poradieTemplate" /> -->
      <DxColumn data-field="Id" caption="Id" :width="100" :visible="false" />
      <DxColumn data-field="NazovZasobnika" caption="Názov zásobníka" />
      <DxColumn
        data-field="DatumVytvorenia"
        caption="Dátum vytvorenia"
        data-type="date"
        :format="dateFormat"
        :allow-editing="false"
        :editor-options="{ max: actualDate() }"
      />
      <DxColumn
        data-field="DatumUpravy"
        caption="Dátum úpravy"
        data-type="date"
        :format="dateFormat"
        :allow-editing="false"
        :editor-options="{ max: actualDate() }"
      />
      <DxColumn data-field="Skratka" caption="Skratka" />
      <!-- <template #poradieTemplate="{ data }">{{ calculatePoradie(data.row.rowIndex) }}</template> -->
    </DxDataGrid>
  </div>
</template>
