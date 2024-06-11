import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const UnauthorizedGuardService: CanActivateFn = () => {
  return localStorage.getItem('accessToken') == null || inject(Router).navigate(['users']);
};