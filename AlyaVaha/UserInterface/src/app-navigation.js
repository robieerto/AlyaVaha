export default [
  {
    text: 'Domov',
    path: '/',
    icon: 'home'
  },
  {
    text: 'Prehľad navažovaní',
    path: '/navazovania',
    icon: 'ordersbox'
  },
  {
    text: 'Štatistiky',
    path: '/statistiky',
    icon: 'chart'
  },
  {
    text: 'Evidencia',
    icon: 'folder',
    items: [
      {
        text: 'Materiály',
        path: '/materialy'
      },
      {
        text: 'Zásobníky',
        path: '/zasobniky'
      },
      {
        text: 'Užívatelia',
        path: '/uzivatelia'
      }
    ]
  },
  {
    text: 'Nastavenia',
    path: '/settings',
    icon: 'preferences',
    admin: true
  }
]
