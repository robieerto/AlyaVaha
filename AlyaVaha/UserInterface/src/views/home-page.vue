<script setup lang="ts">
import { DxButton } from 'devextreme-vue'

import StartNavazovanieModal from '../components/start-navazovanie-modal.vue'
import VstupyStatus from '../components/vstupy-status.vue'
import StavyStatus from '../components/stavy-status.vue'
import DataStatus from '../components/data-status.vue'

import store from '@/store'

function openStartNavazovanieModal() {
  store.isStartNavazovanieModalOpened = true
}
</script>

<template>
  <div>
    <div class="d-flex justify-content-between">
      <div class="d-flex justify-content-start">
        <h2 class="col-6 content-block">Aktuálne dáta</h2>
        <DxButton
          class="col-6 ml-3 mt-3"
          @click="openStartNavazovanieModal"
          text="Štart navažovania"
          type="default"
        />
      </div>
    </div>
    <div class="content-block mt-0">
      <div class="row mb-3">
        <div class="col-auto">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Aktuálna váha (kg)</h5>
              <p class="card-text fw-bold fs-3">{{ store.actualData.BruttoVaha }}</p>
            </div>
          </div>
        </div>
        <div class="col-auto">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Váha navažovania (kg)</h5>
              <p class="card-text fw-bold fs-3">{{ store.actualData.VahaNavazovania }}</p>
            </div>
          </div>
        </div>
        <div class="col-auto">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Súčtová váha (kg)</h5>
              <p class="card-text fw-bold fs-3">{{ store.actualData.CelkovaVaha }}</p>
            </div>
          </div>
        </div>
        <div class="col-auto">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Počet vyrobených cyklov</h5>
              <p class="card-text fw-bold fs-3">
                {{ store.actualData.PocetVyrobenychCyklovVazenia }}
              </p>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <VstupyStatus class="col" />
        <StavyStatus class="col" />
      </div>
      <div class="row mt-3">
        <DataStatus />
      </div>
    </div>
  </div>
  <StartNavazovanieModal v-if="store.isStartNavazovanieModalOpened" />
</template>
