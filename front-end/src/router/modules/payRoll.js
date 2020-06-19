/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout';

const payRollRouter = {
  path: '/pay',
  component: Layout,
  redirect: '/pay/my',
  name: 'payRoll',
  meta: {
    // title: 'position',
    icon: 'international',
  },
  children: [
    {
      path: 'mu',
      component: () => import('@/views/payRoll/index'),
      name: 'PayRoll',
      meta: {
        title: 'payRoll',
      }
    }
  ]
};
export default payRollRouter;
