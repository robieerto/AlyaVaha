<script setup lang="ts">
import { DxButton } from 'devextreme-vue'

import * as VahaAPI from '@/types/vahaTypes'

import StartNavazovanieModal from '../components/start-navazovanie-modal.vue'
import VstupyStatus from '../components/vstupy-status.vue'
import VystupyStatusControl from '../components/vystupy-status-control.vue'
import StavyStatus from '../components/stavy-status.vue'
import DataStatus from '../components/data-status.vue'
import PovelyControl from '../components/povely-control.vue'

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
    <div class="content-block mt-0" v-if="store.connected">
      <div class="mb-2">
        <div class="row">
          <div class="col">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">Aktuálna váha (kg)</h5>
                <p class="card-text fw-bold fs-3">
                  {{
                    store.actualData.BruttoVaha !== -10000
                      ? store.actualData.BruttoVaha
                      : 'pod minimum'
                  }}
                </p>
              </div>
            </div>
          </div>
          <div class="col">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">Váha navažovania (kg)</h5>
                <p class="card-text fw-bold fs-3">
                  {{
                    store.actualData.VahaNavazovania !== -10000
                      ? store.actualData.VahaNavazovania
                      : 'pod minimum'
                  }}
                </p>
              </div>
            </div>
          </div>
          <div class="col">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">Súčtová váha (kg)</h5>
                <p class="card-text fw-bold fs-3">{{ store.actualData.CelkovaVaha }}</p>
              </div>
            </div>
          </div>
          <div class="col">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">Počet dávok</h5>
                <p class="card-text fw-bold fs-3">
                  {{ store.actualData.PocetVyrobenychCyklovVazenia }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="row m-0 mt-2">
        <div class="card col mr-4">
          <div class="col h-100 align-items-center py-2 pl-3">
            <p class="row">Požadovaná váha dávky (kg)</p>
            <p class="row fw-bold">{{ store.actualData.PozadovanaVahaDavky }}</p>
          </div>
        </div>
        <div class="card col mr-4">
          <div class="row h-100 align-items-center py-2 pl-3">
            <p class="row">Požadovaná celková váha (kg)</p>
            <p class="row fw-bold">{{ store.actualData.PozadovanaCelkovaVaha }}</p>
          </div>
        </div>
        <div class="card col mr-4">
          <div class="row h-100 align-items-center py-2 pl-3">
            <p class="row">Požadovaný počet dávok:</p>
            <p class="row fw-bold">{{ store.actualData.PozadovanyPocetDavok }}</p>
          </div>
        </div>
        <div class="card col mr-4">
          <div class="row h-100 align-items-center py-2 pl-3">
            <p class="row">Aktuálny výkon:</p>
            <p class="row fw-bold">{{ store.actualData.VykonAktualny }}</p>
          </div>
        </div>
        <div class="card col">
          <div class="row h-100 align-items-center py-2 pl-3">
            <p class="row">Celkový výkon:</p>
            <p class="row fw-bold">{{ store.actualData.VykonCelkovy }}</p>
          </div>
        </div>
      </div>
      <div class="mt-4">
        <div class="row">
          <div class="col">
            <StavyStatus />
          </div>
          <div class="col">
            <PovelyControl />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col">
            <VstupyStatus />
          </div>
          <div class="col">
            <VystupyStatusControl />
          </div>
        </div>
        <!-- <div class="row mt-3">
          <StavyStatus />
        </div> -->
      </div>
      <div class="row mt-3">
        <DataStatus />
      </div>
      <div class="row mt-5">
        <div class="image-container">
          <div class="vaha-image" />
        </div>
      </div>
    </div>
  </div>
  <StartNavazovanieModal v-if="store.isStartNavazovanieModalOpened" />
</template>

<style lang="scss">
.image-container {
  overflow: hidden;
  width: 500px;
  height: 250px;
  margin: 0 4px;

  .vaha-image {
    width: 100%;
    height: 100%;
    background: url('../assets/vaha.png') no-repeat #fff;
    background-size: cover;
  }
}
</style>
