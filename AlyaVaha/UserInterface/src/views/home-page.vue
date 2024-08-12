<script setup lang="ts">
import { DxButton } from 'devextreme-vue'
import DxLoadIndicator from 'devextreme-vue/load-indicator'
import { reactive, watch } from 'vue'

import * as VahaAPI from '@/types/vahaTypes'

import StartNavazovanieModal from '../components/start-navazovanie-modal.vue'
import VstupyStatus from '../components/vstupy-status.vue'
import VystupyStatusControl from '../components/vystupy-status-control.vue'
import StavyStatus from '../components/stavy-status.vue'
import HlavneUdajeStatus from '../components/hlavne-udaje-status.vue'
import UdajeStatus from '../components/udaje-status.vue'
import DataStatus from '../components/data-status.vue'
import AnimationVaha from '../components/animation-vaha.vue'

import store from '@/store'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  ukoncujePoDokonceniDavky: false,
  ukoncujeOkamzite: false
})

watch(
  () => store.actualData.StavNavazovania,
  () => {
    if (store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieUkoncene) {
      state.ukoncujePoDokonceniDavky = false
      state.ukoncujeOkamzite = false
    }
  }
)

function openStartNavazovanieModal() {
  store.isStartNavazovanieModalOpened = true
}
</script>

<template>
  <div>
    <div class="d-flex justify-content-between">
      <div class="d-flex justify-content-start">
        <h2 class="col content-block">Aktuálne dáta</h2>
        <div class="row" v-if="store.connected">
          <DxButton
            v-if="store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieUkoncene"
            class="col-auto ml-3 mt-3"
            @click="() => sendCommand('SetZeroing', {})"
            text="Nulovanie váhy"
            type="normal"
          />

          <DxButton
            v-if="
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieUkoncene ||
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.Nedefinovane
            "
            class="col-auto ml-3 mt-3"
            @click="openStartNavazovanieModal"
            text="Štart navažovania"
            type="default"
          />

          <DxButton
            v-if="store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieBezi"
            class="col-auto ml-3 mt-3"
            @click="
              () =>
                sendCommand('SetControlValues', {
                  StavNavazovania: VahaAPI.StavNavazovaniaPovel.PrerusenieNavazovania
                })
            "
            text="Prerušenie navažovania"
            type="default"
          />

          <DxButton
            v-if="store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovaniePrerusene"
            class="col-auto ml-3 mt-3"
            @click="
              () =>
                sendCommand('SetControlValues', {
                  StavNavazovania: VahaAPI.StavNavazovaniaPovel.PokracovanieNavazovania
                })
            "
            text="Pokračovanie navažovania"
            type="default"
          />

          <DxButton
            v-if="
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieBezi ||
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovaniePrerusene
            "
            class="col-auto ml-3 mt-3"
            @click="
              () => {
                sendCommand('SetControlValues', {
                  StavNavazovania: VahaAPI.StavNavazovaniaPovel.UkonceniePoUkonceniDavky
                })
                state.ukoncujePoDokonceniDavky = true
              }
            "
            text="Ukončenie po dovážení dávky"
            type="warning"
          >
            <template #content>
              <span>
                <DxLoadIndicator
                  :visible="state.ukoncujePoDokonceniDavky"
                  class="button-indicator mr-3"
                  :height="20"
                  :width="20"
                />
                <span class="dx-button-text">{{
                  state.ukoncujePoDokonceniDavky ? 'Ukončuje' : 'Ukončenie po dovážení dávky'
                }}</span>
              </span>
            </template>
          </DxButton>

          <DxButton
            v-if="
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieBezi ||
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovaniePrerusene
            "
            class="col-auto ml-3 mt-3"
            @click="
              () => {
                sendCommand('SetControlValues', {
                  StavNavazovania: VahaAPI.StavNavazovaniaPovel.OkamziteUkoncenie
                })
                state.ukoncujeOkamzite = true
              }
            "
            text="Okamžité ukončenie"
            type="danger"
          >
            <template #content>
              <span>
                <DxLoadIndicator
                  :visible="state.ukoncujeOkamzite"
                  class="button-indicator mr-3"
                  :height="20"
                  :width="20"
                />
                <span class="dx-button-text">{{
                  state.ukoncujeOkamzite ? 'Ukončuje' : 'Okamžité ukončenie'
                }}</span>
              </span>
            </template>
          </DxButton>
        </div>
      </div>
    </div>
    <div class="content-block mt-0">
      <HlavneUdajeStatus />
      <div class="row mt-3">
        <div class="col mw-450">
          <StavyStatus />
        </div>
        <div class="col-auto">
          <AnimationVaha />
        </div>
        <div class="col mw-450">
          <UdajeStatus />
        </div>
      </div>
      <div v-if="store.isUserAdmin" class="row mt-4">
        <div class="col">
          <VstupyStatus />
        </div>
        <div class="col">
          <VystupyStatusControl />
        </div>
      </div>

      <div v-if="store.isUserAdmin" class="row mt-5">
        <DataStatus />
      </div>
    </div>
  </div>
  <StartNavazovanieModal v-if="store.isStartNavazovanieModalOpened" />
</template>

<style lang="scss">
.mw-450 {
  max-width: 450px;
}
</style>
