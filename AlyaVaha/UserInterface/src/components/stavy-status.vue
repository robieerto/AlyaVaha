<script setup lang="ts">
import store from '@/store'
</script>

<template>
  <section>
    <!-- <h6 class="mb-1">Stavy</h6> -->
    <div class="card card-body p-0">
      <div class="row m-0">
        <!-- <div class="card col-12" v-for="(value, index) in store.actualStateTexts" :key="index">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">{{ index.replace(/([A-Z0-9]+)/g, ' $1').trim() }}:</p>
            <p class="col-6 fw-bold">
              {{ value }}
            </p>
          </div>
        </div> -->
        <div class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav navažovania:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavNavazovania }}
            </p>
          </div>
        </div>
        <div class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav riadenia navažovania:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavRiadeniaNavazovania }}
            </p>
          </div>
        </div>
        <div class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav hornej klapky:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavHornejKlapky }}
            </p>
          </div>
        </div>
        <div class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav dolnej klapky:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavDolnejKlapky }}
            </p>
          </div>
        </div>
        <div class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav sirény:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavSireny }}
            </p>
          </div>
        </div>
        <div class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav vibrátora:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavVibratora }}
            </p>
          </div>
        </div>
        <div class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav váhy:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavVahy }}
            </p>
          </div>
        </div>
        <div v-if="store.isUserAdmin" class="card col-12">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav prevodníka:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavPrevodnika }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
