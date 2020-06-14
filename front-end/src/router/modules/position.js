/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout';

const positionRouter = {
  path: '/position',
  component: Layout,
  redirect: '/position/list',
  name: 'position',
  meta: {
    // title: 'position',
    icon: 'lock',
    roles: ['SuperAdmin', 'HR']
  },
  children: [
    {
      path: 'list',
      component: () => import('@/views/position/index'),
      name: 'Position',
      meta: {
        title: 'position',
      }
    }
  ]
};
export default positionRouter;
