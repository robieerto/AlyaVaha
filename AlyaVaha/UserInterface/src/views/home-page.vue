<script setup lang="ts">
import { reactive } from 'vue'
import { toFloatNumber, toDate } from '@/utils/helpers'
import * as types from '@/types'

const state = reactive({
  data: {} as types.VahaModel
})

function processMessage(message: any) {
  console.log(message)
  state.data = JSON.parse(message)
}

;(window.external as any).receiveMessage(processMessage)
</script>

<template>
  <div>
    <h2 class="content-block">Aktuálne dáta</h2>
    <div class="content-block">
      <div class="row">
        <div class="col-sm-4" v-for="(value, index) in state.data" :key="index">
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
</template>
