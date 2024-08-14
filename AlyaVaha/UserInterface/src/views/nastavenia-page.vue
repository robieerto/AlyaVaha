<script setup>
import 'devextreme/data/odata/store'
import { reactive, watch, computed } from 'vue'
import { DxNumberBox, DxTextBox, DxButton } from 'devextreme-vue'

import store from '@/store'
import { sendCommand } from '@/commandHandler'

sendCommand('GetZariadenia')

const state = reactive({
  deviceName: store.zariadenie?.NazovZariadenia ?? '',
  ipAddress: store.zariadenie?.IpAdresa ?? '',
  port: store.zariadenie?.Port ?? 0,
  isIpAddressValid: true
})

watch(
  () => store.zariadenia,
  () => {
    store.zariadenie = store.zariadenia.find((z) => z.Id === store.zariadenie?.Id)
    state.deviceName = store.zariadenie?.NazovZariadenia ?? ''
    state.ipAddress = store.zariadenie?.IpAdresa ?? ''
    state.port = store.zariadenie?.Port ?? 0
  }
)

const ipAddressValid = computed(() =>
  /^(?:(?:25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}(?:25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])$/.test(
    state.ipAddress
  )
)

async function updateSettings() {
  if (!ipAddressValid.value) {
    return
  }
  sendCommand('UpdateZariadenie', {
    ...store.zariadenie,
    NazovZariadenia: state.deviceName,
    IpAdresa: state.ipAddress,
    Port: state.port
  })
}
</script>

<template>
  <div>
    <h2 class="content-block">Nastavenia</h2>
  </div>

  <div class="my-3 w-100">
    <div class="card card-body">
      <div class="row ml-3 my-2">
        <div class="col-2">
          <div class="align-middle">
            <p class="my-3">Názov:</p>
          </div>
        </div>
        <div class="col-4 ml-2">
          <DxTextBox v-model="state.deviceName" :maxLength="xx" />
        </div>
      </div>

      <div class="row ml-3 my-2">
        <div class="col-2">
          <div class="align-middle">
            <p class="my-3">IP adresa:</p>
          </div>
        </div>
        <div class="col-4 ml-2">
          <DxTextBox v-model="state.ipAddress" :isValid="ipAddressValid" />
          <p v-if="!ipAddressValid" class="text-danger ml-4">Nesprávny formát</p>
        </div>
      </div>

      <div class="row ml-3 my-2">
        <div class="col-2">
          <div class="align-middle">
            <p class="my-3">Port:</p>
          </div>
        </div>
        <div class="col-4 ml-2">
          <DxNumberBox v-model="state.port" min="1" max="65535" />
        </div>
      </div>

      <div class="row ml-3 my-2">
        <div class="col-2 mt-2">
          <DxButton text="Uložiť" type="success" @click="updateSettings" />
        </div>
        <!-- <div class="col-4 ml-2">
          <p class="my-3">Nové nastavenia sa prejavia až po reštartovaní programu</p>
        </div> -->
      </div>
    </div>
  </div>
</template>
