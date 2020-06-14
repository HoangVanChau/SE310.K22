/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout';

const contractRouter = {
  path: '/contract',
  component: Layout,
  redirect: '/contract/list',
  name: 'contract',
  meta: {
    // title: 'position',
    icon: 'example',
    roles: ['SuperAdmin', 'HR']
  },
  children: [
    {
      path: 'list',
      component: () => import('@/views/contract/index'),
      name: 'Contract',
      meta: {
        title: 'contract',
      }
    }
  ]
};
export default contractRouter;
