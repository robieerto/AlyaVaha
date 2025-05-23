<script setup lang="ts">
import { reactive, ref, watch } from 'vue'
import { DxPopup, DxRadioGroup, DxSelectBox, DxNumberBox, DxCheckBox } from 'devextreme-vue'

import store from '@/store'
import { sendCommand } from '@/commandHandler'
import * as VahaAPI from '@/types/vahaTypes'

const radioTypNavazovania = [
  { text: 'Nedefinované', value: 'nedefinovane' },
  { text: 'Požadovaná hmotnosť', value: 'hmotnost' },
  { text: 'Požadovaný počet dávok', value: 'davky' }
]

const radioSpustenieSireny = [
  { text: 'kg od konca', value: 'podlaVahy' },
  { text: 'dávok', value: 'podlaDavok' }
]

const defaultFormData = {
  PozadovanaCelkovaVaha: store.actualData.PozadovanaCelkovaVaha,
  PozadovanyPocetDavok: store.actualData.PozadovanyPocetDavok,
  PozadovanaVahaDavky: store.actualData.PozadovanaVahaDavky,
  CasCykluDavky: store.actualData.CasCykluDavky,
  VahaSirena: store.actualData.VahaSirena,
  PocetDavokSirena: store.actualData.PocetDavokSirena,
  IdCisloMaterialu: store.actualData.IdCisloMaterialu,
  IdOdbernehoMiesta: store.actualData.IdOdbernehoMiesta,
  IdSmerovaciehoMiesta: store.actualData.IdSmerovaciehoMiesta,
  IdCisloPracovnika: store.user?.Id
} as VahaAPI.IVahaModel

const popupRef = ref(null)
var nastaveniaDirty = false

const initSelectedTypNavazovania = () => {
  if (defaultFormData.PozadovanaCelkovaVaha! > 0) {
    return radioTypNavazovania[1].value
  } else if (defaultFormData.PozadovanyPocetDavok! > 0) {
    return radioTypNavazovania[2].value
  } else {
    return radioTypNavazovania[0].value
  }
}

const initSelectedSpustenieSireny = () => {
  if (defaultFormData.VahaSirena! > 0) {
    return radioSpustenieSireny[0].value
  } else if (defaultFormData.PocetDavokSirena! > 0) {
    return radioSpustenieSireny[1].value
  } else {
    return radioSpustenieSireny[0].value
  }
}

const initEnableSirena = () =>
  defaultFormData.VahaSirena! > 0 || defaultFormData.PocetDavokSirena! > 0

const state = reactive({
  formData: defaultFormData,
  selectedTypNavazovania: initSelectedTypNavazovania(),
  selectedSpustenieSireny: initSelectedSpustenieSireny(),
  enableSirena: initEnableSirena()
})

watch(
  () => store.isNavazovanieInitSuccess,
  () => {
    if (store.isNavazovanieInitSuccess) {
      sendCommand('SetControlValues', {
        StavNavazovania: VahaAPI.StavNavazovaniaPovel.StartNavazovania
      })
      closeModal()
    }
  }
)

watch(
  () => store.nastavenia,
  () => {
    nastaveniaDirty = true
  },
  { deep: true }
)

function startAction() {
  // Typ navazovania
  if (state.selectedTypNavazovania === 'hmotnost') {
    state.formData.PozadovanyPocetDavok = 0
  } else if (state.selectedTypNavazovania === 'davky') {
    state.formData.PozadovanaCelkovaVaha = 0
  } else {
    state.formData.PozadovanyPocetDavok = 0
    state.formData.PozadovanaCelkovaVaha = 0
  }

  // Spustenie sirény
  if (!state.enableSirena) {
    state.formData.VahaSirena = 0
    state.formData.PocetDavokSirena = 0
  } else {
    if (state.selectedSpustenieSireny === 'podlaVahy') {
      state.formData.PocetDavokSirena = 0
    } else if (state.selectedSpustenieSireny === 'podlaDavok') {
      state.formData.VahaSirena = 0
    }
  }

  if (nastaveniaDirty) {
    sendCommand('UpdateNastavenia', store.nastavenia)
    nastaveniaDirty = false
  }

  sendCommand('SetValues', state.formData)
}

function typNavazovaniaChanged() {}

function spustenieSirenyChanged() {}

function closeModal() {
  ;(popupRef.value as any).instance.hide()
}

function onModalHidden() {
  store.isStartNavazovanieModalOpened = false
}

store.isNavazovanieInitSuccess = false
</script>

<template>
  <DxPopup
    ref="popupRef"
    :visible="store.isStartNavazovanieModalOpened"
    title="Štart navažovania"
    :showTitle="true"
    :showCloseButton="true"
    :maxWidth="800"
    :maxHeight="1200"
    :onHidden="onModalHidden"
    :toolbarItems="[
      {
        widget: 'dxButton',
        toolbar: 'bottom',
        location: 'after',
        options: {
          text: 'Zrušiť',
          stylingMode: 'contained',
          type: 'info',
          elementAttr: { class: 'mr-1' },
          onClick: closeModal
        }
      },
      {
        widget: 'dxButton',
        toolbar: 'bottom',
        location: 'after',
        options: {
          text: 'Štart',
          stylingMode: 'contained',
          type: 'success',
          elementAttr: { class: 'mx-3' },
          onClick: startAction
        }
      }
    ]"
  >
    <template #content>
      <!-- Typ navažovania -->
      <section class="my-3 w-100">
        <p class="card-title mb-1"><b>Typ navažovania</b></p>
        <div class="card card-body py-0">
          <DxRadioGroup
            v-model:value="state.selectedTypNavazovania"
            :items="radioTypNavazovania"
            value-expr="value"
            layout="vertical"
            item-template="radio"
            @valueChanged="typNavazovaniaChanged"
          >
            <template #radio="{ data }">
              <div class="container w-100">
                <div class="row">
                  <div class="col-4">
                    <div class="align-middle">
                      <p class="my-3">
                        {{ data.text }}
                      </p>
                    </div>
                  </div>
                  <div class="col-4">
                    <DxNumberBox
                      v-if="data.value === 'hmotnost'"
                      v-model:value="state.formData.PozadovanaCelkovaVaha"
                      :disabled="state.selectedTypNavazovania !== 'hmotnost'"
                      :show-spin-buttons="true"
                      mode="number"
                      :step="1"
                      :min="0"
                    />
                    <DxNumberBox
                      v-if="data.value === 'davky'"
                      v-model:value="state.formData.PozadovanyPocetDavok"
                      :disabled="state.selectedTypNavazovania !== 'davky'"
                      :show-spin-buttons="true"
                      mode="number"
                      :step="1"
                      :min="0"
                    />
                  </div>
                  <div v-if="data.value === 'hmotnost'" class="col-1 ps-0">
                    <div class="align-middle">
                      <p class="my-3">kg</p>
                    </div>
                  </div>
                </div>
              </div>
            </template>
          </DxRadioGroup>
        </div>
      </section>

      <!-- Evidenčné parametre -->
      <section class="my-3 w-100">
        <p class="card-title mb-1"><b>Evidenčné parametre</b></p>
        <div class="card card-body">
          <div class="row my-1">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Materiál:</p>
              </div>
            </div>
            <div class="col-6 ml-2">
              <DxSelectBox
                :data-source="store.aktivneMaterialy"
                v-model:value="state.formData.IdCisloMaterialu"
                value-expr="Id"
                display-expr="NazovMaterialu"
                :searchEnabled="true"
              />
            </div>
          </div>

          <div class="row my-1">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Odkiaľ:</p>
              </div>
            </div>
            <div class="col-6 ml-2">
              <DxSelectBox
                :data-source="store.zasobnikyDoVahy"
                v-model:value="state.formData.IdOdbernehoMiesta"
                value-expr="Id"
                display-expr="NazovZasobnika"
                :searchEnabled="true"
              />
            </div>
          </div>

          <div class="row my-1">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Kam:</p>
              </div>
            </div>
            <div class="col-6 ml-2">
              <DxSelectBox
                :data-source="store.zasobnikyZVahy"
                v-model:value="state.formData.IdSmerovaciehoMiesta"
                value-expr="Id"
                display-expr="NazovZasobnika"
                :searchEnabled="true"
              />
            </div>
          </div>
        </div>
      </section>

      <!-- Parametre navažovania -->
      <section class="mt-3 w-100">
        <p class="card-title mb-1"><b>Parametre navažovania</b></p>
        <div class="card card-body">
          <div class="row my-1">
            <div class="col-3">
              <div class="align-middle">
                <p class="my-3">Veľkosť 1 dávky:</p>
              </div>
            </div>
            <div class="col-5 ml-2">
              <DxNumberBox
                v-model:value="state.formData.PozadovanaVahaDavky"
                :show-spin-buttons="true"
                mode="number"
                :step="1"
                :min="0"
              />
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p class="my-3">kg</p>
              </div>
            </div>
          </div>

          <div class="row my-1">
            <div class="col-3">
              <div class="align-middle">
                <p class="my-3">Min. čas navažovania dávky:</p>
              </div>
            </div>
            <div class="col-5 ml-2">
              <DxNumberBox
                v-model:value="state.formData.CasCykluDavky"
                :show-spin-buttons="true"
                mode="number"
                :step="1"
                :min="0"
              />
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p class="my-3">s</p>
              </div>
            </div>
          </div>

          <div class="row my-1">
            <div class="col-auto mt-4">
              <DxCheckBox v-model="state.enableSirena" text="Spustiť sirénu po dosiahnutí:" />
            </div>
            <div class="col">
              <DxRadioGroup
                v-model:value="state.selectedSpustenieSireny"
                :items="radioSpustenieSireny"
                value-expr="value"
                layout="vertical"
                item-template="radio"
                :disabled="!state.enableSirena"
                @valueChanged="spustenieSirenyChanged"
              >
                <template #radio="{ data }">
                  <div class="container w-100">
                    <div class="row">
                      <div class="col-4">
                        <DxNumberBox
                          v-if="data.value === 'podlaVahy'"
                          v-model:value="state.formData.VahaSirena"
                          :disabled="state.selectedSpustenieSireny !== 'podlaVahy'"
                          :show-spin-buttons="true"
                          mode="number"
                          :step="1"
                          :min="0"
                        />
                        <DxNumberBox
                          v-if="data.value === 'podlaDavok'"
                          v-model:value="state.formData.PocetDavokSirena"
                          :disabled="state.selectedSpustenieSireny !== 'podlaDavok'"
                          :show-spin-buttons="true"
                          mode="number"
                          :step="1"
                          :min="0"
                        />
                      </div>
                      <div class="col-3 ps-0">
                        <div class="align-middle">
                          <p class="my-3">{{ data.text }}</p>
                        </div>
                      </div>
                    </div>
                  </div>
                </template>
              </DxRadioGroup>
            </div>
          </div>

          <hr />

          <div class="row mt-3">
            <div class="col-3 mt-3">
              <DxCheckBox
                v-model="store.nastavenia.SirenaPriNepribudani"
                text="Spustiť sirénu pri nepribúdaní"
              />
            </div>
            <div class="col-3 pl-0">
              <DxNumberBox
                v-model:value="store.nastavenia.VahaSirenyPriNepribudani"
                :show-spin-buttons="true"
                mode="number"
                :step="1"
                :min="0"
                :disabled="!store.nastavenia.SirenaPriNepribudani"
              />
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p :class="['my-3', !store.nastavenia.SirenaPriNepribudani && 'disabled-text']">
                  kg
                </p>
              </div>
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p :class="['my-3', !store.nastavenia.SirenaPriNepribudani && 'disabled-text']">
                  po čase
                </p>
              </div>
            </div>
            <div class="col-3 pl-0">
              <DxNumberBox
                v-model:value="store.nastavenia.CasSirenyPriNepribudani"
                :show-spin-buttons="true"
                mode="number"
                :step="1"
                :min="0"
                :disabled="!store.nastavenia.SirenaPriNepribudani"
              />
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p :class="['my-3', !store.nastavenia.SirenaPriNepribudani && 'disabled-text']">
                  s
                </p>
              </div>
            </div>
          </div>
          <div class="row mt-3">
            <div class="col-3 mt-3">
              <DxCheckBox
                v-model="store.nastavenia.SirenaPriNeodbudani"
                text="Spustiť sirénu pri neodbúdaní"
              />
            </div>
            <div class="col-3 pl-0">
              <DxNumberBox
                v-model:value="store.nastavenia.VahaSirenyPriNeodbudani"
                :show-spin-buttons="true"
                mode="number"
                :step="1"
                :min="0"
                :disabled="!store.nastavenia.SirenaPriNeodbudani"
              />
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p :class="['my-3', !store.nastavenia.SirenaPriNeodbudani && 'disabled-text']">
                  kg
                </p>
              </div>
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p :class="['my-3', !store.nastavenia.SirenaPriNeodbudani && 'disabled-text']">
                  po čase
                </p>
              </div>
            </div>
            <div class="col-3 pl-0">
              <DxNumberBox
                v-model:value="store.nastavenia.CasSirenyPriNeodbudani"
                :show-spin-buttons="true"
                mode="number"
                :step="1"
                :min="0"
                :disabled="!store.nastavenia.SirenaPriNeodbudani"
              />
            </div>
            <div class="col-1 ps-0">
              <div class="align-middle">
                <p :class="['my-3', !store.nastavenia.SirenaPriNeodbudani && 'disabled-text']">s</p>
              </div>
            </div>
          </div>

          <!-- <div class="row mt-3">
            <div class="col-auto">
              <div class="row mb-3">
                <div class="col-auto">
                  <DxCheckBox
                    v-model="store.nastavenia.SirenaPriNeodbudani"
                    text="Spustiť sirénu pri neodbúdaní"
                  />
                </div>
              </div>
              <div class="row my-4">
                <div class="col-auto">
                  <DxCheckBox
                    v-model="store.nastavenia.SirenaPriNepribudani"
                    text="Spustiť sirénu pri nepribúdaní"
                  />
                </div>
              </div>
            </div>
            <div class="col-8">
              <div class="row my-1">
                <div class="col-3">
                  <div class="align-middle">
                    <p class="my-3">Čas spustenia:</p>
                  </div>
                </div>
                <div class="col-5 ml-2">
                  <DxNumberBox
                    v-model:value="store.nastavenia.CasSireny"
                    :show-spin-buttons="true"
                    mode="number"
                    :step="1"
                    :min="0"
                  />
                </div>
                <div class="col-1 ps-0">
                  <div class="align-middle">
                    <p class="my-3">s</p>
                  </div>
                </div>
              </div>
            </div>
          </div> -->
        </div>
      </section>

      <!-- Action Buttons -->
      <!-- <div class="row justify-content-end">
        <div class="col-auto">
          <DxButton text="Zrušiť" @click="closeModal" />
        </div>
        <div class="col-auto">
          <DxButton text="Štart" type="success" @click="startAction" />
        </div>
      </div> -->
    </template>
  </DxPopup>
</template>

<style scoped lang="scss">
.disabled-text {
  color: #b6b6b6;
}
</style>
