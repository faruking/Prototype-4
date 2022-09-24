using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed = 5.0f;
    private float powerupStrength = 10.0f;
    public bool hasPowerup  = false;
    public GameObject powerupIndicator;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward* forwardInput * speed);
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Powerup")){
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }
    IEnumerator PowerupCountdownRoutine(){
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }
    private void OnCollisionEnter(Collision collider){

        if(collider.gameObject.CompareTag("Enemy") && hasPowerup){
            Rigidbody enemyRigidbody = collider.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collider.gameObject.transform.position - transform.position;

            Debug.Log("Collided with: " + collider.gameObject.name + " with powerup set to: " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength,ForceMode.Impulse);
        }
    }
}
