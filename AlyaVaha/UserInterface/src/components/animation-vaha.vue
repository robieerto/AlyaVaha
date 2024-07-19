<script setup lang="ts">
import 'animate.css'
import store from '@/store'
import { computed, reactive, watchEffect } from 'vue'

import * as VahaAPI from '@/types/vahaTypes'

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
const isSirenaVypnuta = computed(
  () => store.actualData.StavSireny === VahaAPI.StavSireny.SirenaVypnuta
)

// vibrator
const isVibratorZapnuty = computed(
  () => store.actualData.StavVibratora === VahaAPI.StavVibratora.VibratorZapnuty
)
const isVibratorVypnuty = computed(
  () => store.actualData.StavVibratora === VahaAPI.StavVibratora.VibratorVypnuty
)

const classHornaKlapka = computed(() => ({
  open: isHornaKlapkaOtvorena.value,
  closed: isHornaKlapkaZatvorena.value,
  'animate-opening': isHornaKlapkaSaOtvara.value,
  animate__rotateOutDownRight: isHornaKlapkaSaOtvara.value,
  'animate-closing': isHornaKlapkaSaZatvara.value,
  animate__rotateInDownLeft: isHornaKlapkaSaZatvara.value,
  error: isHornaKlapkaVPoruche.value,
  animate__bounceIn: isHornaKlapkaVPoruche.value,
  undefined: isHornaKlapkaNedefinovana.value
}))

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

const classSirena = computed(() => ({
  'is-on': isSirenaZapnuta.value,
  animate__flash: isSirenaZapnuta.value,
  'is-off': isSirenaVypnuta.value
}))

const classVibrator = computed(() => ({
  'is-on': isVibratorZapnuty.value,
  animate__headShake: isVibratorZapnuty.value,
  'is-off': isVibratorVypnuty.value
}))
</script>

<template>
  <div>
    <div class="vaha-container">
      <div class="vaha-image" />
      <div class="alya-logo" />
      <div class="sirena animate__animated animate__infinite" :class="classSirena" />
      <div class="vibrator animate__animated animate__infinite" :class="classVibrator" />
      <div class="horna-klapka animate__animated animate__infinite" :class="classHornaKlapka" />
      <div class="dolna-klapka animate__animated animate__infinite" :class="classDolnaKlapka" />
      <div class="aktualna-vaha text-center">
        <span class="text-center">
          {{ store.actualData.BruttoVaha !== -10000 ? store.actualData.BruttoVaha : 'pod minimum' }}
        </span>
      </div>
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
  }

  .vibrator {
    position: absolute;
    top: 190px;
    left: 410px;
    width: 50px;
    height: 50px;
    background: url('../assets/vibration-new.png') no-repeat;
    background-size: cover;
  }
}

.is-on {
}

.is-off {
  filter: grayscale(100%);
}

.aktualna-vaha {
  position: absolute;
  top: 110px;
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
}

.dolna-klapka {
  position: absolute;
  top: 242px;
  left: 210px;
  width: 86px;
  height: 18px;
  margin-top: 20px;
  transform-origin: center;
}

.open {
  background: linear-gradient(#2eb52c, #157347);
  transform: rotate3d(0, 0, 1, -90deg);
}

.closed {
  background: linear-gradient(#de8e8c, #bb2d3b);
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
