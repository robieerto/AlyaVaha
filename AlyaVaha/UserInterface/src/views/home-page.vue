<script setup lang="ts">
import DxButton from 'devextreme-vue/button'
import StartNavazovanieModal from '../components/start-navazovanie-modal.vue'

import store from '@/store'

function openStartNavazovanieModal() {
  store.isStartNavazovanieModalOpened = true
}
</script>

<template>
  <div>
    <h2 class="content-block">Aktuálne dáta</h2>
    <div class="content-block">
      <div class="row">
        <DxButton class="col-3 ml-3" @click="openStartNavazovanieModal" text="Štart navažovania" />
      </div>
      <div class="row">
        <div class="col-4" v-for="(value, index) in store.actualData" :key="index">
          <div class="card mt-3">
            <div class="card-body">
              <h5 class="card-title">{{ index }}</h5>
              <p class="card-text fw-bold fs-3">{{ value }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <StartNavazovanieModal v-if="store.isStartNavazovanieModalOpened" />
</template>
