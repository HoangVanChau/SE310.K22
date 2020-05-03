/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout'

const teamRouter = {
  path: '/team',
  component: Layout,
  redirect: '/team/list',
  name: 'Team',
  meta: {
    title: 'Team',
    icon: 'table'
  },
  children: [
    {
      path: 'list',
      component: () => import('@/views/team/index'),
      name: 'LstTeam',
      meta: { title: 'lstTeam' }
    }
  ]
}
export default teamRouter
