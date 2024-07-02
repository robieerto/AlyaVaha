<script setup lang="ts">
import { changeDigitalOutput } from '@/commandHandler'
import store from '@/store'
</script>

<template>
  <section>
    <h6 class="mb-1">Výstupy</h6>
    <div class="card card-body p-0">
      <div class="row m-0">
        <div class="card col-6" v-for="(value, index) in store.actualOutputs" :key="index">
          <div class="row h-100 d-flex align-items-center py-2 pl-3">
            <button
              type="button"
              :class="'col btn btn-' + (value ? 'success' : 'secondary')"
              @click="() => changeDigitalOutput(index)"
            >
              {{ index.replace(/([A-Z0-9]+)/g, ' $1').trim() }}
            </button>
            <p
              :class="'col fw-bold status contact-status status-' + value?.toString().toLowerCase()"
            >
              {{ value ? 'Zapnutý' : 'Vypnutý' }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped lang="scss">
@use 'sass:math';

.btn {
  min-height: 48px;
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
