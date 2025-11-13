using UnityEngine;

public class JetpackPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if player already has a jetpack
            if (other.GetComponent<Jetpack>() == null)
            {
                // Attach Jetpack component
                Jetpack jetpack = other.gameObject.AddComponent<Jetpack>();
                jetpack.Initialize(other.gameObject);

                //Transfer Audioclip from pickup
                Jetpack pickupJetpack = GetComponent<Jetpack>();
                if (pickupJetpack != null && pickupJetpack.jetpackClip != null)
                {
                    jetpack.jetpackClip = pickupJetpack.jetpackClip;
                }


                // Optionally play pickup FX or sound here

                //UI Popup
                gameManager.instance.jetpackPopup.SetActive(true);

                // Destroy the pickup object
                Destroy(gameObject);
            }
        }
    }
}
