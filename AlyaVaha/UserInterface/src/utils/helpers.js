const floatFormat = '#,##0.00'
const dateFormat = 'd.M.yyyy H:mm'
const timeFormat = 'H:mm'

const actualDate = new Date(new Date().setUTCHours(0, 0, 0, 0))

const toFloatNumber = (value, digits) =>
  value != null && value != undefined && !isNaN(value)
    ? parseFloat(value)
        .toFixed(digits)
        .toLocaleString('sk-SK', { minimumFractionDigits: 1, maximumFractionDigits: digits })
    : ''

const toDate = (value) => {
  if (!value) return ''
  const date = new Date(value).toLocaleString('sk-SK').replaceAll('. ', '.')
  if (date != 'Invalid Date') return date
  else return ''
}

const toCustomDate = (value) => {
  if (!value) return ''
  const date = new Date(value).toISOString().replace('T', ' ').replace('Z', '')
  if (date != 'Invalid Date') return date
  else return ''
}

const getTomorrow = (date) => {
  const tomorrow = new Date(date)
  tomorrow.setDate(tomorrow.getDate() + 1)
  return tomorrow
}

export {
  actualDate,
  floatFormat,
  dateFormat,
  timeFormat,
  toFloatNumber,
  toDate,
  toCustomDate,
  getTomorrow
}
