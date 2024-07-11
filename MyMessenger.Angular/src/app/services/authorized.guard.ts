import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const AuthorizedGuard: CanActivateFn = () => {
  return (
    localStorage.getItem('accessToken') == null ||
    inject(Router).navigate(['users'])
  );
};
