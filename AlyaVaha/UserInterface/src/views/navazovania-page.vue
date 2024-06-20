<script setup>
import 'devextreme/data/odata/store'
import DxDataGrid, {
  DxColumn,
  DxFilterRow,
  DxPager,
  DxPaging,
  DxEditing,
  DxLookup
} from 'devextreme-vue/data-grid'
import { DxLoadPanel } from 'devextreme-vue/load-panel'
import { reactive } from 'vue'

import { dateFormat, actualDate } from '@/utils/helpers'
import store from '@/store'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  dataGridInstance: null
})

sendCommand('GetNavazovania')

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

// const calculatePoradie = (rowIndex) =>
//   rowIndex + state.dataGridInstance.pageIndex() * state.dataGridInstance.pageSize()
</script>

<template>
  <div>
    <h2 class="content-block">Prehľad navažovaní</h2>
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
      <DxEditing :allow-updating="false" :allow-deleting="false" :allow-adding="true" />
      <!-- <dx-column caption="Riadok" :allow-search="false" :allow-sorting="false" :alignment="'right'" cell-template="poradieTemplate" /> -->
      <DxColumn data-field="Id" caption="Id" :width="100" :visible="false" />
      <DxColumn
        data-field="CasStartu"
        caption="Čas štartu"
        data-type="datetime"
        :format="dateFormat"
        :allow-editing="false"
        :editor-options="{ max: actualDate() }"
      />
      <DxColumn
        data-field="CasKonca"
        caption="Čas konca"
        data-type="datetime"
        :format="dateFormat"
        :allow-editing="false"
        :editor-options="{ max: actualDate() }"
      />
      <DxColumn data-field="ZariadenieId" caption="Zariadenie">
        <DxLookup :data-source="store.zariadenia" value-expr="Id" display-expr="NazovZariadenia" />
      </DxColumn>
      <DxColumn data-field="NavazeneMnozstvo" caption="Navážené množstvo" data-type="number" />
      <DxColumn data-field="NavazenyPocetDavok" caption="Navážený počet dávok" data-type="number" />
      <DxColumn data-field="PozadovaneMnozstvo" caption="Požadované množstvo" data-type="number" />
      <DxColumn
        data-field="PozadovanyPocetDavok"
        caption="Požadovaný počet dávok"
        data-type="number"
      />
      <DxColumn data-field="VelkostDavky" caption="Veľkosť dávky" data-type="number" />
      <DxColumn data-field="OdkialId" caption="Zásobník odkiaľ">
        <DxLookup :data-source="store.zasobniky" value-expr="Id" display-expr="NazovZasobnika" />
      </DxColumn>
      <DxColumn data-field="KamId" caption="Zásobník kam">
        <DxLookup :data-source="store.zasobniky" value-expr="Id" display-expr="NazovZasobnika" />
      </DxColumn>
      <!-- <template #poradieTemplate="{ data }">{{ calculatePoradie(data.row.rowIndex) }}</template> -->
    </DxDataGrid>
  </div>
</template>
