using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;
    public float pickupTime = 4f;

    private Rigidbody rb;
    private bool doOnce;
    private bool active;
    private float multiplier = 2f;


    public GameObject shot;
    public Transform[] shotSpawns;
    public float fireRate;

    private float nextFire;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach (var shotSpawn in shotSpawns)
            {
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }
                GetComponent<AudioSource>().Play ();
        }

        if (Input.GetKey("escape"))
            Application.Quit();

    }



    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        doOnce = true;
        active = false;

    }

    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
             Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
             0.0f,
             Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup") && doOnce == true && active == false)
        {
            Destroy(other.gameObject);
            doOnce = false;
            active = true;
            StartCoroutine(Pickup());
        }
    }

    IEnumerator Pickup()
    {
        speed *= multiplier;

        yield return new WaitForSeconds(pickupTime);

        speed /= multiplier;
        doOnce = true;
        active = false;

    }

}
