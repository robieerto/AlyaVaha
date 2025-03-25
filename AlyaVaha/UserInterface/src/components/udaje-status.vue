<script setup lang="ts">
import store from '@/store'
import { formatDateTime } from '@/utils/helpers'
</script>

<template>
  <section>
    <div class="row m-0">
      <div class="card col">
        <div class="row h-100 d-flex align-items-center py-2 pl-3">
          <p class="col-6">Dátum a čas štartu navažovania:</p>
          <p class="col-6 fw-bold">
            {{ formatDateTime(store.actualData.DatumCasStartuNavazovania) }}
          </p>
        </div>
      </div>
    </div>

    <div class="row">
      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Materiál:</p>
            <p class="col-6 fw-bold">
              {{
                store.materialy.find((m) => m.Id === store.actualData.IdCisloMaterialu)
                  ?.NazovMaterialu
              }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Odkiaľ:</p>
            <p class="col-6 fw-bold">
              {{
                store.zasobniky.find((m) => m.Id === store.actualData.IdOdbernehoMiesta)
                  ?.NazovZasobnika
              }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Kam:</p>
            <p class="col-6 fw-bold">
              {{
                store.zasobniky.find((m) => m.Id === store.actualData.IdSmerovaciehoMiesta)
                  ?.NazovZasobnika
              }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav hornej klapky:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavHornejKlapky }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav dolnej klapky:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavDolnejKlapky }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav sirény:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavSireny }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav vibrátora:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavVibratora }}
            </p>
          </div>
        </div>
      </div>

      <div class="row m-0">
        <div class="card col">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">Stav váhy:</p>
            <p class="col-6 fw-bold">
              {{ store.actualStateTexts.StavVahy }}
            </p>
          </div>
        </div>
      </div>

      <div v-if="store.isUserAdmin" class="row m-0">
        <div class="card col">
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
