import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const AuthorizedGuardService: CanActivateFn = () => {
  return !!localStorage.getItem('accessToken') || inject(Router).navigate(['login']);
};