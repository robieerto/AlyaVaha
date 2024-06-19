<script setup>
import 'devextreme/data/odata/store'
import DxDataGrid, { DxColumn, DxFilterRow, DxPager, DxPaging } from 'devextreme-vue/data-grid'
import { DxLoadPanel } from 'devextreme-vue/load-panel'
import { reactive } from 'vue'

import { dateFormat, actualDate } from '@/utils/helpers'
import store from '@/store'
import { sendCommand } from '@/messageHandler'

const state = reactive({
  dataGridInstance: null
})

sendCommand('GetMaterialy')

function onDataGridInitialized(e) {
  state.dataGridInstance = e.component
}

const calculatePoradie = (rowIndex) =>
  rowIndex + state.dataGridInstance.pageIndex() * state.dataGridInstance.pageSize()
</script>

<template>
  <div>
    <h2 class="content-block">Evidencia materiálov</h2>

    <DxLoadPanel v-model:visible="store.materialyLoading" />

    <DxDataGrid
      v-if="!store.materialyLoading"
      class="dx-card wide-card"
      :data-source="store.materialy"
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
    >
      <DxPaging :page-size="10" />
      <DxPager :show-page-size-selector="true" :show-info="true" />
      <DxFilterRow :visible="true" />
      <!-- <dx-column caption="Riadok" :allow-search="false" :allow-sorting="false" :alignment="'right'" cell-template="poradieTemplate" /> -->
      <DxColumn data-field="Id" caption="Id" :visible="false" />
      <DxColumn data-field="NazovMaterialu" caption="Názov materiálu" />
      <DxColumn
        data-field="DatumVytvorenia"
        caption="Dátum vytvorenia"
        data-type="datetime"
        :format="dateFormat"
        :editor-options="{ max: actualDate }"
      />
      <DxColumn data-field="HmotnostMaterialu" caption="Hmotnosť materiálu" />
      <!-- <template #poradieTemplate="{ data }">{{ calculatePoradie(data.row.rowIndex) }}</template> -->
    </DxDataGrid>
  </div>
</template>
