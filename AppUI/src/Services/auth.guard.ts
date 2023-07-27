import { CanActivateFn } from '@angular/router';
import { Inject, inject } from '@angular/core';
import { Router } from '@angular/router';

export const authGuard:CanActivateFn = () =>
{
  const router = inject(Router);
  setInterval(() => {
    router.navigate(['/']);
    }, 2000)
    return true;
  };
