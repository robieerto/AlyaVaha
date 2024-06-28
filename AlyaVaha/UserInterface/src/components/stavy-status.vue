<script setup lang="ts">
import store from '@/store'
</script>

<template>
  <section>
    <h6 class="mb-1">Stavy</h6>
    <div class="card card-body p-0">
      <div class="row m-0">
        <div class="card col-6" v-for="(value, index) in store.actualStateTexts" :key="index">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <p class="col-6">{{ index.replace(/([A-Z0-9]+)/g, ' $1').trim() }}:</p>
            <p class="col-6 fw-bold">
              {{ value }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
