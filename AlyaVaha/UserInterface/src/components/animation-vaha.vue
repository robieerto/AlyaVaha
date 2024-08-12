<script setup lang="ts">
import 'animate.css'
import store from '@/store'
import { computed, reactive, watchEffect } from 'vue'

import * as VahaAPI from '@/types/vahaTypes'
import { sendCommand } from '@/commandHandler'

const state = reactive({
  stavHornejKlapky: store.actualData.StavHornejKlapky,
  stavDolnejKlapky: store.actualData.StavDolnejKlapky
})

watchEffect(() => {
  state.stavHornejKlapky = store.actualData.StavHornejKlapky
  state.stavDolnejKlapky = store.actualData.StavDolnejKlapky
})

// horna klapka
const isHornaKlapkaOtvorena = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.KlapkaOtvorena
)
const isHornaKlapkaZatvorena = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.KlapkaZatvorena
)
const isHornaKlapkaSaOtvara = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.KlapkaSaOtvara
)
const isHornaKlapkaSaZatvara = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.KlapkaSaZatvara
)
const isHornaKlapkaVPoruche = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.KlapkaVPoruche
)
const isHornaKlapkaNedefinovana = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.Nedefinovane
)
const isKlapkaSaOtvaraNaStred = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.KlapkaSaOtvaraNaStred
)
const IsKlapkaNaStred = computed(
  () => state.stavHornejKlapky === VahaAPI.StavKlapky.KlapkaJeNaStred
)

// dolna klapka
const isDolnaKlapkaOtvorena = computed(
  () => state.stavDolnejKlapky === VahaAPI.StavKlapky.KlapkaOtvorena
)
const isDolnaKlapkaZatvorena = computed(
  () => state.stavDolnejKlapky === VahaAPI.StavKlapky.KlapkaZatvorena
)
const isDolnaKlapkaSaOtvara = computed(
  () => state.stavDolnejKlapky === VahaAPI.StavKlapky.KlapkaSaOtvara
)
const isDolnaKlapkaSaZatvara = computed(
  () => state.stavDolnejKlapky === VahaAPI.StavKlapky.KlapkaSaZatvara
)
const isDolnaKlapkaVPoruche = computed(
  () => state.stavDolnejKlapky === VahaAPI.StavKlapky.KlapkaVPoruche
)
const isDolnaKlapkaNedefinovana = computed(
  () => state.stavDolnejKlapky === VahaAPI.StavKlapky.Nedefinovane
)

// sirena
const isSirenaZapnuta = computed(
  () => store.actualData.StavSireny === VahaAPI.StavSireny.SirenaZapnuta
)
const isSirenaZapnutaPrerusovane = computed(
  () => store.actualData.StavSireny === VahaAPI.StavSireny.SirenaZapnutaPrerusovane
)
const isSirenaVypnuta = computed(
  () =>
    store.actualData.StavSireny === VahaAPI.StavSireny.SirenaVypnuta ||
    store.actualData.StavSireny === undefined ||
    store.actualData.StavSireny === null
)

// vibrator
const isVibratorZapnuty = computed(
  () => store.actualData.StavVibratora === VahaAPI.StavVibratora.VibratorZapnuty
)
const isVibratorVypnuty = computed(
  () =>
    store.actualData.StavVibratora === VahaAPI.StavVibratora.VibratorVypnuty ||
    store.actualData.StavVibratora === undefined ||
    store.actualData.StavVibratora === null
)

// ovladanie klapiek
const isHornaKlapkaZatvorenaControl = computed(
  () =>
    store.actualData.StavHornejKlapky === VahaAPI.StavKlapky.Nedefinovane ||
    store.actualData.StavHornejKlapky === VahaAPI.StavKlapky.KlapkaZatvorena ||
    store.actualData.StavHornejKlapky === VahaAPI.StavKlapky.KlapkaSaZatvara
)
const isDolnaKlapkaZatvorenaControl = computed(
  () =>
    store.actualData.StavDolnejKlapky === VahaAPI.StavKlapky.Nedefinovane ||
    store.actualData.StavDolnejKlapky === VahaAPI.StavKlapky.KlapkaZatvorena ||
    store.actualData.StavDolnejKlapky === VahaAPI.StavKlapky.KlapkaSaZatvara
)

const classHornaKlapka = computed(() => ({
  open: isHornaKlapkaOtvorena.value,
  closed: isHornaKlapkaZatvorena.value,
  naStred: IsKlapkaNaStred.value,
  'animate-opening': isHornaKlapkaSaOtvara.value,
  animate__rotateOutDownRight: isHornaKlapkaSaOtvara.value,
  'animate-closing': isHornaKlapkaSaZatvara.value,
  animate__rotateInDownLeft: isHornaKlapkaSaZatvara.value,
  'animate-opening-na-stred': isKlapkaSaOtvaraNaStred.value,
  animate__rotateInDownRight: isKlapkaSaOtvaraNaStred.value,
  error: isHornaKlapkaVPoruche.value,
  animate__bounceIn: isHornaKlapkaVPoruche.value,
  undefined: isHornaKlapkaNedefinovana.value
}))

const classHornaKlapkaText = computed(() => [
  (isHornaKlapkaOtvorena.value || isHornaKlapkaSaOtvara.value) && 'text-success',
  (isHornaKlapkaZatvorena.value || isHornaKlapkaSaZatvara.value) && 'text-danger',
  isHornaKlapkaVPoruche.value && 'text-danger'
])

const classDolnaKlapka = computed(() => ({
  open: isDolnaKlapkaOtvorena.value,
  closed: isDolnaKlapkaZatvorena.value,
  'animate-opening': isDolnaKlapkaSaOtvara.value,
  animate__rotateOutDownRight: isDolnaKlapkaSaOtvara.value,
  'animate-closing': isDolnaKlapkaSaZatvara.value,
  animate__rotateInDownLeft: isDolnaKlapkaSaZatvara.value,
  error: isDolnaKlapkaVPoruche.value,
  animate__bounceIn: isDolnaKlapkaVPoruche.value,
  undefined: isDolnaKlapkaNedefinovana.value
}))

const classDolnaKlapkaText = computed(() => [
  (isDolnaKlapkaOtvorena.value || isDolnaKlapkaSaOtvara.value) && 'text-success',
  (isDolnaKlapkaZatvorena.value || isDolnaKlapkaSaZatvara.value) && 'text-danger',
  isDolnaKlapkaVPoruche.value && 'text-danger'
])

const classSirena = computed(() => ({
  'is-on': isSirenaZapnuta.value || isSirenaZapnutaPrerusovane.value,
  animate__heartBeat: isSirenaZapnuta.value,
  animate__flash: isSirenaZapnutaPrerusovane.value,
  'is-off': isSirenaVypnuta.value
}))

const classVibrator = computed(() => ({
  'is-on': isVibratorZapnuty.value,
  animate__headShake: isVibratorZapnuty.value,
  'is-off': isVibratorVypnuty.value
}))

const setHornaKlapkaControl = () => {
  if (!store.connected) return

  sendCommand('SetValues', {
    StavHornejKlapky: isHornaKlapkaZatvorenaControl.value
      ? VahaAPI.StavKlapkyPovel.OtvorKlapku
      : VahaAPI.StavKlapkyPovel.ZatvorKlapku
  })
}

const setDolnaKlapkaControl = () => {
  if (!store.connected) return

  sendCommand('SetValues', {
    StavDolnejKlapky: isDolnaKlapkaZatvorenaControl.value
      ? VahaAPI.StavKlapkyPovel.OtvorKlapku
      : VahaAPI.StavKlapkyPovel.ZatvorKlapku
  })
}

const setSirenaControl = () => {
  if (!store.connected) return

  sendCommand('SetValues', {
    StavSireny: isSirenaVypnuta.value
      ? VahaAPI.StavSirenyPovel.ZapniSirenu
      : VahaAPI.StavSirenyPovel.VypniSirenu
  })
}

const setVibratorControl = () => {
  if (!store.connected) return

  sendCommand('SetValues', {
    StavVibratora: isVibratorVypnuty.value
      ? VahaAPI.StavVibratoraPovel.ZapniVibrator
      : VahaAPI.StavVibratoraPovel.VypniVibrator
  })
}
</script>

<template>
  <div>
    <div
      class="horna-klapka-control"
      :class="isHornaKlapkaVPoruche && 'klapka-porucha'"
      @click="setHornaKlapkaControl"
    >
      <p class="p-0 fs-5 fw-bold text-center" :class="classHornaKlapkaText">
        {{ store.actualStateTexts.StavHornejKlapky }}
      </p>
    </div>
    <div class="vaha-container">
      <div class="vaha-image" />
      <div class="alya-logo" />
      <div
        class="sirena animate__animated animate__infinite"
        :class="classSirena"
        @click="setSirenaControl"
      />
      <div
        class="vibrator animate__animated animate__infinite"
        :class="classVibrator"
        @click="setVibratorControl"
      />
      <div
        class="horna-klapka animate__animated animate__infinite"
        :class="classHornaKlapka"
        @click="setHornaKlapkaControl"
      />
      <div
        class="dolna-klapka animate__animated animate__infinite"
        :class="classDolnaKlapka"
        @click="setDolnaKlapkaControl"
      />
      <div class="aktualna-vaha text-center">
        <span class="text-center">
          {{ store.actualData.BruttoVaha !== -10000 ? store.actualData.BruttoVaha : 'pod minimum' }}
        </span>
      </div>
    </div>
    <div
      class="dolna-klapka-control"
      :class="isDolnaKlapkaVPoruche && 'klapka-porucha'"
      @click="setDolnaKlapkaControl"
      :disabled="!store.connected"
    >
      <p class="p-0 fs-5 fw-bold text-center" :class="classDolnaKlapkaText">
        {{ store.actualStateTexts.StavDolnejKlapky }}
      </p>
    </div>
  </div>
</template>

<style lang="scss">
.vaha-container {
  position: relative;
  overflow: hidden;
  margin: 0 4px;
  width: 500px;
  height: 330px;
  padding-top: 30px;

  .vaha-image {
    width: 500px;
    height: 250px;
    background: url('../assets/vaha.png') no-repeat;
    background-size: cover;
  }

  .alya-logo {
    position: absolute;
    top: 170px;
    left: 190px;
    width: 120px;
    height: 40px;
    background: url('../assets/alya-logo.png') no-repeat;
    background-size: cover;
  }

  .sirena {
    position: absolute;
    top: 190px;
    left: 50px;
    width: 50px;
    height: 50px;
    background: url('../assets/alarm.png') no-repeat;
    background-size: cover;
    cursor: pointer;
  }

  .vibrator {
    position: absolute;
    top: 190px;
    left: 410px;
    width: 50px;
    height: 50px;
    background: url('../assets/vibration-new.png') no-repeat;
    background-size: cover;
    cursor: pointer;
  }
}

.is-on {
}

.is-off {
  filter: grayscale(100%);
}

.dolna-klapka-control {
  position: relative;
  width: 200px;
  top: -16px;
  left: 157px;
  cursor: pointer;
}

.horna-klapka-control {
  position: relative;
  width: 200px;
  top: 10px;
  left: 157px;
  cursor: pointer;
  z-index: 100;
}

.klapka-porucha {
  border: 2px solid red;
}

.aktualna-vaha {
  position: absolute;
  top: 111px;
  left: 152px;
  width: 200px;
  font-size: 30px;
  font-weight: bold;
  color: white;
}

.horna-klapka {
  position: absolute;
  top: 24px;
  left: 210px;
  width: 86px;
  height: 18px;
  margin-top: 20px;
  transform-origin: center;
  cursor: pointer;
}

.dolna-klapka {
  position: absolute;
  top: 242px;
  left: 210px;
  width: 86px;
  height: 18px;
  margin-top: 20px;
  transform-origin: center;
  cursor: pointer;
}

.open {
  background: linear-gradient(#2eb52c, #157347);
  transform: rotate3d(0, 0, 1, -90deg);
}

.closed {
  background: linear-gradient(#de8e8c, #bb2d3b);
}

.naStred {
  transform: rotate3d(0, 0, 1, -45deg);
  background: linear-gradient(#ffc107, orange);
}

.error {
  background: linear-gradient(#de8e8c, #bb2d3b);
  --animate-duration: 1.5s;
}

.undefined {
  background: linear-gradient(#8e8e8c, #2d2d3b);
}

.animate-opening {
  --animate-duration: 1.5s;
  background: linear-gradient(#2eb52c, #157347);
}

.animate-closing {
  --animate-duration: 1.5s;
  background: linear-gradient(#de8e8c, #bb2d3b);
}

.animate-opening-na-stred {
  --animate-duration: 1.5s;
  background: linear-gradient(#ffc107, orange);
}

@keyframes rotateOutDownRight {
  from {
    opacity: 0;
  }

  to {
    transform: rotate3d(0, 0, 1, -90deg);
    opacity: 1;
  }
}

@keyframes rotateInDownLeft {
  from {
    transform: rotate3d(0, 0, 1, -90deg);
    opacity: 0;
  }

  to {
    transform: translate3d(0, 0, 0);
    opacity: 1;
  }
}

@keyframes rotateInDownRight {
  from {
    opacity: 0;
  }

  to {
    transform: rotate3d(0, 0, 1, -45deg);
    opacity: 1;
  }
}

@keyframes bounceIn {
  from,
  20%,
  40%,
  60%,
  80%,
  to {
    animation-timing-function: cubic-bezier(0.215, 0.61, 0.355, 1);
  }

  0% {
    opacity: 1;
    transform: scale3d(0.3, 0.3, 0.3);
  }

  20% {
    transform: scale3d(1.1, 1.1, 1.1);
  }

  40% {
    transform: scale3d(0.9, 0.9, 0.9);
  }

  60% {
    opacity: 1;
    transform: scale3d(1.03, 1.03, 1.03);
  }

  80% {
    transform: scale3d(0.97, 0.97, 0.97);
  }

  to {
    opacity: 1;
    transform: scale3d(1, 1, 1);
  }
}
</style>
