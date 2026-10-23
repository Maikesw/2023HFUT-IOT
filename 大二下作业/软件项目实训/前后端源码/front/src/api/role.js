import request from '@/utils/request'
export default {
  getRolePermissions,
  editRolePermission
}
function getRolePermissions(roleId) {
  return request({
    url: '/role/getRolePermissions',
    method: 'post',
    data: { roleId }
  })
}

function editRolePermission(roleId, permissions) {
  return request({
    url: '/role/editRolePermissions',
    method: 'post',
    data: { roleId, permissions }
  })
}
