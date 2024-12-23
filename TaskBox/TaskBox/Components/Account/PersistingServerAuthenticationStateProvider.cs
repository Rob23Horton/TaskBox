﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Diagnostics;

using TaskBox.Shared.Models;

namespace TaskBox.Components.Account
{
	// This is a server-side AuthenticationStateProvider that uses PersistentComponentState to flow the
	// authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
	internal sealed class PersistingServerAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
	{
		private readonly PersistentComponentState state;
		private readonly IdentityOptions options;

		private readonly PersistingComponentStateSubscription subscription;

		private Task<AuthenticationState>? authenticationStateTask;

		public PersistingServerAuthenticationStateProvider(
			PersistentComponentState persistentComponentState,
			IOptions<IdentityOptions> optionsAccessor)
		{
			state = persistentComponentState;
			options = optionsAccessor.Value;

			AuthenticationStateChanged += OnAuthenticationStateChanged;
			subscription = state.RegisterOnPersisting(OnPersistingAsync);
		}

		private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
		{
			authenticationStateTask = task;
		}

		private async Task OnPersistingAsync()
		{
			if (authenticationStateTask is null)
			{
				throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
			}

			var authenticationState = await authenticationStateTask;
			var principal = authenticationState.User;

			if (principal.Identity?.IsAuthenticated == true)
			{
				var userId = principal.FindFirst(options.ClaimsIdentity.UserIdClaimType)?.Value;
				var userName = principal.FindFirst(options.ClaimsIdentity.UserNameClaimType)?.Value;

				if (userId != null && userName != null)
				{
					state.PersistAsJson(nameof(User), new User
					{
						Id = int.Parse(userId),
						UserName = userName
					});
				}
			}
		}

		public void Dispose()
		{
			subscription.Dispose();
			AuthenticationStateChanged -= OnAuthenticationStateChanged;
		}
	}
}