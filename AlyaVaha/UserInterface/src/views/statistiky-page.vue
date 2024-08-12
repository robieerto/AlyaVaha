<script setup>
import 'devextreme/data/odata/store'
import { reactive, watch, ref } from 'vue'
import { DxDateBox, DxSelectBox, DxButton } from 'devextreme-vue'
import { DxCheckBox } from 'devextreme-vue/check-box'
import { DxLoadIndicator } from 'devextreme-vue/load-indicator'
import Vue3Html2pdf from 'vue3-html2pdf'
import jsPDF from 'jspdf'

import store from '@/store'
import { sendCommand } from '@/commandHandler'
import {
  toFloatNumber,
  toDate,
  toTime,
  toTimezoneDate,
  getTomorrow,
  getIncrementByMinute
} from '@/utils/helpers'
import ReportPdf from './report-pdf.vue'

const html2pdfRef = ref()

const getZariadenia = () => [{ Id: 0, NazovZariadenia: '(Všetko)' }, ...store.zariadenia]
const getUzivatelia = () => [{ Id: 0, Login: '(Všetko)' }, ...store.aktivniUzivatelia]
const getMaterialy = () => [{ Id: 0, NazovMaterialu: '(Všetko)' }, ...store.aktivneMaterialy]
const getZasobnikyDoVahy = () => [{ Id: 0, NazovZasobnika: '(Všetko)' }, ...store.zasobnikyDoVahy]
const getZasobnikyZVahy = () => [{ Id: 0, NazovZasobnika: '(Všetko)' }, ...store.zasobnikyZVahy]

sendCommand('GetZariadenia')

const state = reactive({
  pocetNavazeni: 0,
  navazeneMnozstvo: 0,
  navazenyPocetDavok: 0,
  zariadenia: getZariadenia(),
  uzivatelia: getUzivatelia(),
  materialy: getMaterialy(),
  zasobnikyDoVahy: getZasobnikyDoVahy(),
  zasobnikyZVahy: getZasobnikyZVahy(),
  isActualData: true,
  filterOptions: {
    zariadenie: 0,
    datumOd: null,
    datumDo: null,
    uzivatel: 0,
    material: 0,
    odkial: 0,
    kam: 0
  },
  filterOptionsStr: {
    zariadenie: '',
    datumOd: '',
    datumDo: '',
    uzivatel: '',
    material: '',
    odkial: '',
    kam: ''
  }
})

watch(
  () => store.zariadenia,
  () => {
    state.zariadenia = getZariadenia()
  }
)

watch(
  () => store.aktivniUzivatelia,
  () => {
    state.uzivatelia = getUzivatelia()
  }
)

watch(
  () => store.aktivneMaterialy,
  () => {
    state.materialy = getMaterialy()
  }
)

watch(
  () => store.zasobnikyDoVahy,
  () => {
    state.zasobnikyDoVahy = getZasobnikyDoVahy()
  }
)

watch(
  () => store.zasobnikyZVahy,
  () => {
    state.zasobnikyZVahy = getZasobnikyZVahy()
  }
)

watch(
  () => store.statistiky,
  () => {
    state.pocetNavazeni = store.statistiky?.[0]
    state.navazeneMnozstvo = store.statistiky?.[1]
    state.navazenyPocetDavok = store.statistiky?.[2]
    store.statistikyLoading = false
    state.isActualData = true
  }
)

watch(
  () => state.filterOptions,
  () => {
    state.isActualData = false
  },
  { deep: true }
)

watch(
  () => store.isDateTimeStatistiky,
  () => {
    state.filterOptions.datumOd = null
    state.filterOptions.datumDo = null
  }
)

// function getCobminedFilter, which builds a filter object based on the state.filterOptions
function getCombinedFilter() {
  const filter = []
  if (state.filterOptions.zariadenie) {
    filter.push(['ZariadenieId', '=', state.filterOptions.zariadenie])
  }
  if (state.filterOptions.uzivatel) {
    filter.push(['UzivatelId', '=', state.filterOptions.uzivatel])
  }
  if (state.filterOptions.datumOd) {
    var date = toTimezoneDate(state.filterOptions.datumOd)
    filter.push(['DatumStartu', '>=', date])
  }
  if (state.filterOptions.datumDo) {
    var next = store.isDateTimeStatistiky
      ? getIncrementByMinute(state.filterOptions.datumDo)
      : getTomorrow(state.filterOptions.datumDo)
    var dateAfter = toTimezoneDate(next)
    filter.push(['DatumStartu', '<', dateAfter])
  }
  if (state.filterOptions.material) {
    filter.push(['MaterialId', '=', state.filterOptions.material])
  }
  if (state.filterOptions.odkial) {
    filter.push(['OdkialId', '=', state.filterOptions.odkial])
  }
  if (state.filterOptions.kam) {
    filter.push(['KamId', '=', state.filterOptions.kam])
  }

  return filter
}

function getStatistics() {
  store.statistikyLoading = true
  const dataSourceLoadOptions = {}
  dataSourceLoadOptions.Filter = getCombinedFilter()
  dataSourceLoadOptions.TotalSummary = [
    { Selector: 'NavazeneMnozstvo', SummaryType: 'sum' },
    { Selector: 'NavazenyPocetDavok', SummaryType: 'sum' }
  ]
  dataSourceLoadOptions.RequireTotalCount = true
  dataSourceLoadOptions.Skip = 0
  dataSourceLoadOptions.Take = 1
  sendCommand('GetStatistiky', dataSourceLoadOptions)
}

getStatistics()

function filterToStr() {
  const filterStr = {
    zariadenie: state.filterOptions.zariadenie
      ? state.zariadenia.find((zariadenie) => zariadenie.Id === state.filterOptions.zariadenie)
          .NazovZariadenia
      : '(Všetko)',
    datumOd:
      toDate(state.filterOptions.datumOd) +
      (store.isDateTimeStatistiky ? String(' ' + toTime(state.filterOptions.datumOd)) : ''),
    datumDo:
      toDate(state.filterOptions.datumDo) +
      (store.isDateTimeStatistiky ? String(' ' + toTime(state.filterOptions.datumDo)) : ''),
    uzivatel: state.filterOptions.uzivatel
      ? store.aktivniUzivatelia.find((uzivatel) => uzivatel.Id === state.filterOptions.uzivatel)
          .Login
      : '(Všetko)',
    material: state.filterOptions.material
      ? store.aktivneMaterialy.find((material) => material.Id === state.filterOptions.material)
          .NazovMaterialu
      : '(Všetko)',
    odkial: state.filterOptions.odkial
      ? store.zasobnikyDoVahy.find((zasobnik) => zasobnik.Id === state.filterOptions.odkial)
          .NazovZasobnika
      : '(Všetko)',
    kam: state.filterOptions.kam
      ? store.zasobnikyZVahy.find((zasobnik) => zasobnik.Id === state.filterOptions.kam)
          .NazovZasobnika
      : '(Všetko)'
  }
  state.filterOptionsStr = filterStr
}

function exportToPdf() {
  const generatePdf = () => {
    try {
      filterToStr()
      html2pdfRef.value.generatePdf()
    } catch (e) {
      console.log(e)
    }
  }

  if (state.isActualData) {
    generatePdf()
  } else {
    const stop = watch(
      () => state.isActualData,
      () => {
        if (!store.statistikyLoading) {
          generatePdf()
          stop()
        }
      }
    )
    getStatistics()
  }
}
</script>

<template>
  <div class="d-flex justify-content-between" style="margin-bottom: -16px">
    <div class="d-flex justify-content-start">
      <h2 class="content-block">Štatistiky</h2>
    </div>
    <div>
      <button type="button" class="btn btn-primary h-50 m-3 mr-0" @click="exportToPdf">
        Exportovať do PDF
      </button>
    </div>
  </div>
  <div class="my-3 w-100">
    <div class="card card-body">
      <div class="row">
        <div class="col">
          <div class="row ml-3 my-2">
            <div class="col-3">
              <div class="align-middle">
                <p class="my-3">Zariadenie:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.zariadenia"
                v-model:value="state.filterOptions.zariadenie"
                value-expr="Id"
                display-expr="NazovZariadenia"
                :searchEnabled="true"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-3">
              <div class="align-middle">
                <p class="my-3">Dátum od:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxDateBox
                v-model="state.filterOptions.datumOd"
                :input-attr="{ 'aria-label': store.isDateTimeStatistiky ? 'Date Time' : 'Date' }"
                :type="store.isDateTimeStatistiky ? 'datetime' : 'date'"
                :show-clear-button="true"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-3">
              <div class="align-middle">
                <p class="my-3">Dátum do:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxDateBox
                v-model="state.filterOptions.datumDo"
                :input-attr="{ 'aria-label': store.isDateTimeStatistiky ? 'Date Time' : 'Date' }"
                :type="store.isDateTimeStatistiky ? 'datetime' : 'date'"
                :show-clear-button="true"
              />
            </div>
          </div>

          <div class="d-flex justify-content-end mt-4 mr-5">
            <DxCheckBox
              text="Filtrovať podľa času"
              class="mr-5"
              v-model:value="store.isDateTimeStatistiky"
            />
          </div>
        </div>

        <div class="col">
          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Užívateľ:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.uzivatelia"
                v-model:value="state.filterOptions.uzivatel"
                value-expr="Id"
                display-expr="Login"
                :searchEnabled="true"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Materiál:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.materialy"
                v-model:value="state.filterOptions.material"
                value-expr="Id"
                display-expr="NazovMaterialu"
                :searchEnabled="true"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Odkiaľ:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.zasobnikyDoVahy"
                v-model:value="state.filterOptions.odkial"
                value-expr="Id"
                display-expr="NazovZasobnika"
                :searchEnabled="true"
              />
            </div>
          </div>

          <div class="row ml-3 my-2">
            <div class="col-2">
              <div class="align-middle">
                <p class="my-3">Kam:</p>
              </div>
            </div>
            <div class="col-8 ml-2">
              <DxSelectBox
                :data-source="state.zasobnikyZVahy"
                v-model:value="state.filterOptions.kam"
                value-expr="Id"
                display-expr="NazovZasobnika"
                :searchEnabled="true"
              />
            </div>
          </div>
        </div>

        <div class="row ml-3 my-0">
          <div class="col-12 mt-2">
            <DxButton text="Načítať" type="success" @click="getStatistics" />
          </div>
        </div>

        <div
          v-if="store.statistikyLoading"
          class="mt-5 mb-3 d-flex justify-content-center align-items-center"
        >
          <DxLoadIndicator :height="96" :width="96" />
        </div>

        <div v-if="!store.statistikyLoading" class="row mt-5 mb-3 mx-2">
          <div class="col">
            <div class="card">
              <div class="row h-100 d-flex align-items-center py-2 pl-3">
                <h5 class="card-title">Počet navažovaní</h5>
                <p class="card-text fw-bold fs-3">
                  {{ state.pocetNavazeni }}
                </p>
              </div>
            </div>
          </div>
          <div class="col">
            <div class="card">
              <div class="row h-100 d-flex align-items-center py-2 pl-3">
                <h5 class="card-title">Navážená hmotnosť (kg)</h5>
                <p class="card-text fw-bold fs-3">
                  {{ toFloatNumber(state.navazeneMnozstvo, 1) }}
                </p>
              </div>
            </div>
          </div>
          <div class="col">
            <div class="card">
              <div class="row h-100 d-flex align-items-center py-2 pl-3">
                <h5 class="card-title">Navážený počet dávok</h5>
                <p class="card-text fw-bold fs-3">
                  {{ state.navazenyPocetDavok }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div>
    <vue3-html2pdf
      :show-layout="false"
      :float-layout="true"
      :enable-download="true"
      :preview-modal="false"
      :paginate-elements-by-height="1400"
      filename="AlyaVaha-Statistiky"
      :pdf-quality="2"
      :manual-pagination="false"
      pdf-format="a4"
      :pdf-margin="10"
      pdf-orientation="landscape"
      pdf-content-width="1100px"
      ref="html2pdfRef"
    >
      <template v-slot:pdf-content>
        <ReportPdf
          :filterStr="state.filterOptionsStr"
          :pocetNavazeni="state.pocetNavazeni"
          :navazeneMnozstvo="state.navazeneMnozstvo"
          :navazenyPocetDavok="state.navazenyPocetDavok"
        />
      </template>
    </vue3-html2pdf>
  </div>
</template>
