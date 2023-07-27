import { CanActivateFn } from '@angular/router';
import {inject} from '@angular/core';
import { AccountService } from './account.service';
import { Router } from '@angular/router';

export const authGuard:CanActivateFn = () =>
  {

  return true;
  };
