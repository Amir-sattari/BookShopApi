using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Helpers
{
    public static class OwnershipGuard
    {
        public static bool TryResolveCurrentUser(
            this ControllerBase controller,
            string? requestedUserId,
            out string userId,
            out ActionResult? error)
        {
            var currentUserId = controller.User.GetUserId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                userId = string.Empty;
                error = controller.Unauthorized();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(requestedUserId) && requestedUserId != currentUserId)
            {
                userId = currentUserId;
                error = controller.Forbid();
                return false;
            }

            userId = currentUserId;
            error = null;
            return true;
        }
    }
}
