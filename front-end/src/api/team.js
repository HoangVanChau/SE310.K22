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
export function updateTeam(teamId, dataParam) {
  const data = {
    TeamName: dataParam.teamName,
    LeaderId: dataParam.leaderId,
    TeamAvatarImageId: dataParam.teamAvatarImageId || null
  };
  return request({
    url: `/api/teams/${teamId}`,
    method: 'patch',
    data
  });
}
export function createTeam(dataParam) {
  const data = {
    TeamName: dataParam.teamName,
    LeaderId: dataParam.leaderId,
    TeamAvatarImageId: dataParam.teamAvatarImageId || null
  };

  return request({
    url: `/api/teams`,
    method: 'post',
    data
  });
}
export function addUserToTeam(teamId, userId) {
  const data = {
    UserId: userId
  };
  return request({
    url: `/api/teams/${teamId}`,
    method: 'post',
    data
  });
}
export function removeUserToTeam(teamId, userId) {
  const data = {
    UserId: userId
  };
  return request({
    url: `/api/teams/${teamId}`,
    method: 'delete',
    data
  });
}
export function changeLeader(teamId, leaderId) {
  const data = {
    UserId: leaderId
  };
  return request({
    url: `/api/teams/${teamId}/leader`,
    method: 'patch',
    data
  });
}
