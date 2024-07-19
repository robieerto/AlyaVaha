<script setup lang="ts">
import { computed } from 'vue'
import * as VahaAPI from '@/types/vahaTypes'
import store from '@/store'
import { sendCommand } from '@/commandHandler'

const isSirenaVypnuta = computed(
  () => !!store.actualData.StavSireny === !!VahaAPI.StavSireny.SirenaVypnuta
)
const isVibratorVypnuty = computed(
  () => !!store.actualData.StavVibratora === !!VahaAPI.StavVibratora.VibratorVypnuty
)
const isHornaKlapkaZatvorena = computed(
  () =>
    store.actualData.StavHornejKlapky === VahaAPI.StavKlapky.Nedefinovane ||
    store.actualData.StavHornejKlapky === VahaAPI.StavKlapky.KlapkaZatvorena ||
    store.actualData.StavHornejKlapky === VahaAPI.StavKlapky.KlapkaSaZatvara
)
const isDolnaKlapkaZatvorena = computed(
  () =>
    store.actualData.StavDolnejKlapky === VahaAPI.StavKlapky.Nedefinovane ||
    store.actualData.StavDolnejKlapky === VahaAPI.StavKlapky.KlapkaZatvorena ||
    store.actualData.StavDolnejKlapky === VahaAPI.StavKlapky.KlapkaSaZatvara
)
</script>

<template>
  <section>
    <!-- <h6 class="mb-1">Povely</h6> -->
    <div class="card card-body p-0">
      <div class="row m-0">
        <div class="card col-6">
          <div class="col h-100 align-items-center py-2">
            <div class="row m-0">
              <p class="col-6 p-0 text-right">Horná klapka:</p>
              <p class="col-6 fw-bold text-left">
                {{ store.actualStateTexts.StavHornejKlapky }}
              </p>
            </div>
            <div class="row">
              <button
                :class="
                  'w-50 mt-2 mx-auto btn btn-' + (isHornaKlapkaZatvorena ? 'success' : 'danger')
                "
                @click="
                  () =>
                    sendCommand('SetValues', {
                      StavHornejKlapky: isHornaKlapkaZatvorena
                        ? VahaAPI.StavKlapkyPovel.OtvorKlapku
                        : VahaAPI.StavKlapkyPovel.ZatvorKlapku
                    })
                "
                :disabled="!store.connected"
              >
                {{ isHornaKlapkaZatvorena ? 'Otvoriť' : 'Zatvoriť' }}
              </button>
            </div>
          </div>
        </div>

        <div class="card col-6">
          <div class="col h-100 align-items-center py-2">
            <div class="row m-0">
              <p class="col-6 text-right">Siréna:</p>
              <p class="col-6 p-0 fw-bold text-left">
                {{ store.actualStateTexts.StavSireny }}
              </p>
            </div>
            <div class="row">
              <button
                :class="'w-50 mt-2 mx-auto btn btn-' + (isSirenaVypnuta ? 'success' : 'danger')"
                @click="
                  () =>
                    sendCommand('SetValues', {
                      StavSireny: isSirenaVypnuta
                        ? VahaAPI.StavSirenyPovel.ZapniSirenu
                        : VahaAPI.StavSirenyPovel.VypniSirenu
                    })
                "
                :disabled="!store.connected"
              >
                {{ isSirenaVypnuta ? 'Zapnúť' : 'Vypnúť' }}
              </button>
            </div>
          </div>
        </div>

        <div class="card col-6">
          <div class="col h-100 align-items-center py-2">
            <div class="row m-0">
              <p class="col-6 p-0 text-right">Dolná klapka:</p>
              <p class="col-6 fw-bold text-left">
                {{ store.actualStateTexts.StavDolnejKlapky }}
              </p>
            </div>
            <div class="row">
              <button
                :class="
                  'w-50 mt-2 mx-auto btn btn-' + (isDolnaKlapkaZatvorena ? 'success' : 'danger')
                "
                @click="
                  () =>
                    sendCommand('SetValues', {
                      StavDolnejKlapky: isDolnaKlapkaZatvorena
                        ? VahaAPI.StavKlapkyPovel.OtvorKlapku
                        : VahaAPI.StavKlapkyPovel.ZatvorKlapku
                    })
                "
                :disabled="!store.connected"
              >
                {{ isDolnaKlapkaZatvorena ? 'Otvoriť' : 'Zatvoriť' }}
              </button>
            </div>
          </div>
        </div>

        <div class="card col-6">
          <div class="col h-100 align-items-center py-2">
            <div class="row m-0">
              <p class="col-6 text-right">Vibrátor:</p>
              <p class="col-6 p-0 fw-bold text-left">
                {{ store.actualStateTexts.StavVibratora }}
              </p>
            </div>
            <div class="row">
              <button
                :class="'w-50 mt-2 mx-auto btn btn-' + (isVibratorVypnuty ? 'success' : 'danger')"
                @click="
                  () =>
                    sendCommand('SetValues', {
                      StavVibratora: isVibratorVypnuty
                        ? VahaAPI.StavVibratoraPovel.ZapniVibrator
                        : VahaAPI.StavVibratoraPovel.VypniVibrator
                    })
                "
                :disabled="!store.connected"
              >
                {{ isVibratorVypnuty ? 'Zapnúť' : 'Vypnúť' }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped lang="scss">
@use 'sass:math';

.btn {
  min-height: 2cap;
  min-width: 112px;
}

.contact-status {
  font-size: 13px;

  &::before {
    --diameter: 10px;

    content: ' ';
    width: var(--diameter);
    height: var(--diameter);
    border-radius: calc(var(--diameter) / 2);
    margin-right: calc(var(--diameter) / 2);
    display: inline-block;
    align-self: center;
  }

  :deep(&.input) {
    display: block;
    padding: 15px 16px 14px;
  }
}
</style>
