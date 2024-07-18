export default [
  {
    text: 'Domov',
    path: '/home',
    icon: 'home'
  },
  {
    text: 'Prehľad navažovaní',
    path: '/navazovania',
    icon: 'ordersbox'
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
      }
    ]
  },
  {
    text: 'Nastavenia',
    path: '/settings',
    icon: 'preferences'
  }
]
