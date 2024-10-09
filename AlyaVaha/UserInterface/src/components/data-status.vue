<script setup lang="ts">
import { DxButton } from 'devextreme-vue'

import store from '@/store'
import { sendCommand } from '@/commandHandler'
</script>

<template>
  <section>
    <h6 class="mb-1">Hodnoty</h6>
    <div class="card card-body p-0">
      <div class="row m-0">
        <div class="card col-3" v-for="(value, index) in store.actualData" :key="index">
          <div class="row h-100 d-flex align-items-center py-1 pl-3">
            <p class="col-6">{{ index.replace(/([A-Z0-9]+)/g, ' $1').trim() }}:</p>
            <p class="col-6 fw-bold">
              {{ value }}
            </p>
          </div>
        </div>
      </div>
    </div>
    <DxButton
      class="col-auto ml-0 mt-3"
      @click="() => sendCommand('ResetTabulky', {})"
      text="Reset tabuliek zariadenia"
      type="danger"
    />
  </section>
</template>
