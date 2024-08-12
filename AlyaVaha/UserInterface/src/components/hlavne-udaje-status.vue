<script setup lang="ts">
import store from '@/store'
</script>

<template>
  <section>
    <div class="mb-2">
      <div class="row">
        <div class="col">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Aktuálna hmotnosť (kg)</h5>
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
              <h5 class="card-title">Hmotnosť dávky (kg)</h5>
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
  </section>
</template>
