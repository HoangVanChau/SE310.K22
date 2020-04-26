import request from '@/utils/request';

export function getAllTeam() {
  return request({
    url: '/api/teams',
    method: 'get'
  });
}
export function getTeam(teamId) {
  return request({
    url: `/api/teams/${teamId}`,
    method: 'get'
  });
}
export function updateTeam(teamId, data) {
  return request({
    url: `/api/teams/${teamId}`,
    method: 'patch',
    data
  });
}
export function createTeam(data) {
  return request({
    url: `/api/teams`,
    method: 'post',
    data
  });
}
export function addUserToTeam(teamId, userId) {
  return request({
    url: `/api/teams/${teamId}`,
    method: 'post',
    userId
  });
}
export function removeUserToTeam(teamId, userId) {
  return request({
    url: `/api/teams/${teamId}`,
    method: 'delete',
    userId
  });
}
