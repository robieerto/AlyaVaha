<script setup lang="ts">
import store from '@/store'
</script>

<template>
  <section>
    <div class="row">
      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav navažovania:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavNavazovania }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav riadenia navažovania:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavRiadeniaNavazovania }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Požadovaná hmotnosť dávky (kg):</p>
            <p class="col-6 fw-bold">
              {{ store.actualData.PozadovanaVahaDavky }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Požadovaná celková hmotnosť (kg):</p>
            <p class="col-6 fw-bold">
              {{ store.actualData.PozadovanaCelkovaVaha }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Požadovaný počet dávok:</p>
            <p class="col-6 fw-bold">
              {{ store.actualData.PozadovanyPocetDavok }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Min. čas dávky (s):</p>
            <p class="col-6 fw-bold">
              {{ store.actualData.CasCykluDavky }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Celkový výkon:</p>
            <p class="col-6 fw-bold">
              {{
                store.actualData.VykonCelkovy !== undefined
                  ? Number(Number(store.actualData.VykonCelkovy / 1000).toFixed(2))
                  : ''
              }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Aktuálny výkon:</p>
            <p class="col-6 fw-bold">
              {{ store.actualData.VykonAktualny }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
