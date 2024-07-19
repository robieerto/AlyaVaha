<script setup lang="ts">
import { DxButton } from 'devextreme-vue'

import * as VahaAPI from '@/types/vahaTypes'

import StartNavazovanieModal from '../components/start-navazovanie-modal.vue'
import VstupyStatus from '../components/vstupy-status.vue'
import VystupyStatusControl from '../components/vystupy-status-control.vue'
import StavyStatus from '../components/stavy-status.vue'
import HlavneUdajeStatus from '../components/hlavne-udaje-status.vue'
import UdajeStatus from '../components/udaje-status.vue'
import DataStatus from '../components/data-status.vue'
import PovelyControl from '../components/povely-control.vue'
import AnimationVaha from '../components/animation-vaha.vue'

import store from '@/store'
import { sendCommand } from '@/commandHandler'

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
              () =>
                sendCommand('SetControlValues', {
                  StavNavazovania: VahaAPI.StavNavazovaniaPovel.UkonceniePoUkonceniDavky
                })
            "
            text="Ukončenie po ukončení dávky"
            type="warning"
          />

          <DxButton
            v-if="
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieBezi ||
              store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovaniePrerusene
            "
            class="col-auto ml-3 mt-3"
            @click="
              () =>
                sendCommand('SetControlValues', {
                  StavNavazovania: VahaAPI.StavNavazovaniaPovel.OkamziteUkoncenie
                })
            "
            text="Okamžité ukončenie"
            type="danger"
          />
        </div>
      </div>
    </div>
    <div class="content-block mt-0">
      <HlavneUdajeStatus />
      <div class="row mt-3">
        <div class="col-auto">
          <AnimationVaha />
        </div>
        <div class="col">
          <div class="row mt-2">
            <div class="col-7">
              <PovelyControl />
              <div class="mt-2">
                <StavyStatus />
              </div>
            </div>
            <div class="col-5">
              <UdajeStatus />
            </div>
          </div>
          <div class="row"></div>
        </div>
      </div>
      <div class="row mt-4">
        <div class="col">
          <VstupyStatus />
        </div>
        <div class="col">
          <VystupyStatusControl />
        </div>
      </div>

      <div class="row mt-3">
        <DataStatus />
      </div>
    </div>
  </div>
  <StartNavazovanieModal v-if="store.isStartNavazovanieModalOpened" />
</template>
