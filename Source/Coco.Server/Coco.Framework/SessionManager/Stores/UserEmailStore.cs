﻿//using Coco.Framework.Models;
//using System.Threading.Tasks;
//using System;
//using Microsoft.AspNetCore.Identity;

//namespace Coco.Framework.SessionManager.Stores
//{
//    public class UserEmailStore : IUserEmailStore<ApplicationUser>, IDisposable
//    {
//        private bool _isDisposed;

//        public async Task<string> GetEmailAsync(ApplicationUser user)
//        {
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            return await Task.FromResult(user.Email);
//        }

//        public async Task<ICommonResult> SendForgotPasswordAsync(ApplicationUser user)
//        {
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            return await Task.FromResult(CommonResult.Success(user.ActiveUserStamp));
//        }

//        public virtual Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
//        {
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            return Task.FromResult(user.IsEmailConfirmed);
//        }

//        /// <summary>
//        /// Throws if this class has been disposed.
//        /// </summary>
//        protected void ThrowIfDisposed()
//        {
//            if (_isDisposed)
//            {
//                throw new ObjectDisposedException(GetType().Name);
//            }
//        }

//        /// <summary>
//        /// Dispose the store
//        /// </summary>
//        public void Dispose()
//        {
//            _isDisposed = true;
//        }
//    }
//}
