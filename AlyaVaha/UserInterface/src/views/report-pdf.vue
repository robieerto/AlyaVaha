<script setup lang="ts">
import { toFloatNumber } from '@/utils/helpers'

const props = defineProps<{
  filterStr: {
    zariadenie: String
    uzivatel: String
    datumOd: String
    datumDo: String
    material: String
    odkial: String
    kam: String
  }
  pocetNavazeni: Number
  navazeneMnozstvo: Number
  navazenyPocetDavok: Number
}>()
</script>

<template>
  <div style="margin: 12px 12px">
    <h4 class="text-center fw-bold mb-3">Štatistiky navažovania</h4>
    <hr class="bg-dark border-2 border-top border-dark" />
    <div class="container fs-4">
      <!-- <div class="row mb-5">
        <div class="col">
          <strong>Alya váha</strong>
        </div>
      </div> -->
      <div class="row mb-1">
        <div class="col-4">
          <strong>Obdobie:</strong>
        </div>
        <div class="col-1">
          <strong>Od:</strong>
        </div>
        <div class="col-2">
          {{ props.filterStr.datumOd }}
        </div>
        <div class="col-1">
          <strong>Do:</strong>
        </div>
        <div class="col-2">
          {{ props.filterStr.datumDo }}
        </div>
      </div>
      <div class="row mb-1">
        <div class="col-4">
          <strong>Zariadenie:</strong>
        </div>
        <div class="col-4">
          {{ props.filterStr.zariadenie }}
        </div>
      </div>
      <div class="row mb-1">
        <div class="col-4">
          <strong>Užívateľ:</strong>
        </div>
        <div class="col-4">
          {{ props.filterStr.uzivatel }}
        </div>
      </div>
      <div class="row mb-1">
        <div class="col-4">
          <strong>Materiál:</strong>
        </div>
        <div class="col-4">
          {{ props.filterStr.material }}
        </div>
      </div>
      <div class="row mb-1">
        <div class="col-4">
          <strong>Zásobník odkiaľ:</strong>
        </div>
        <div class="col-4">
          {{ props.filterStr.odkial }}
        </div>
      </div>
      <div class="row mb-4">
        <div class="col-4">
          <strong>Zásobník kam:</strong>
        </div>
        <div class="col-4">
          {{ props.filterStr.kam }}
        </div>
      </div>
      <div class="d-flex justify-content-between row"></div>
    </div>
    <hr class="bg-dark border-2 border-top border-dark" />
    <div class="container fs-4">
      <div class="row mb-1">
        <div class="col-4">
          <strong>Počet navažovaní:</strong>
        </div>
        <div class="col-4">
          {{ props.pocetNavazeni }}
        </div>
      </div>
      <div class="row mb-1">
        <div class="col-4">
          <strong>Navážené množstvo (kg):</strong>
        </div>
        <div class="col-4">
          {{ toFloatNumber(props.navazeneMnozstvo, 1) }}
        </div>
      </div>
      <div class="row mb-1">
        <div class="col-4">
          <strong>Navážený počet dávok:</strong>
        </div>
        <div class="col-4">
          {{ props.navazenyPocetDavok }}
        </div>
      </div>
    </div>
  </div>
</template>
