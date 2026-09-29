// 3-body Starter Code
// Fall 2026. IMDM 327
// Instructor. Myungin Lee
using TMPro.EditorUtilities;
using UnityEngine;

public class ThreeBody : MonoBehaviour
{
    private const float G = 200f; // Gravitational constant for this simulation, not the real-world value.
    public float minimumDistance = 1f;
    public float fasterTime = 1f;
    public float separationForce = 1f;
    
    BodyProperty[] bp;
    private int numberOfSphere = 100;
    class BodyProperty // why struct?
    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct
        public GameObject body;
        public float mass;
        public Vector3 velocity;
        public Vector3 acceleration;
    }

    void Start()
    {
        // Allocate an array to store each body's properties.
        bp = new BodyProperty[numberOfSphere];
        // Loop generating the gameobject and assign initial conditions (type, position, (mass/velocity/acceleration)
        for (int i = 0; i < numberOfSphere; i++)
        {
            // Our gameobjects are created here:
            bp[i] = new BodyProperty();
            bp[i].body = GameObject.CreatePrimitive(PrimitiveType.Sphere); // why sphere? try different options.
            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html

            // initial conditions
            float r = 100f;
            // position is (x,y,z). In this case, I want to plot them on the circle with r

            // ******** Fill in this part ********
            float theta = 2 * Mathf.PI / numberOfSphere * i;
            bp[i].body.transform.position = new Vector3(r * Mathf.Cos(theta) + Random.Range(-10, 10), 
                                                        r * Mathf.Sin(theta) + Random.Range(-10, 10), 
                                                        180 + Random.Range(-10, 10));
            // z = 180 places the bodies in front of a camera near the origin looking along +Z. Try other positions too.

            bp[i].velocity = Vector3.zero; // Try different initial condition
            bp[i].mass = 1; // Simplified. Try different initial condition


            // + This is just pretty trails
            TrailRenderer trailRenderer = bp[i].body.AddComponent<TrailRenderer>();
            // Configure the TrailRenderer's properties
            trailRenderer.time = 100.0f;  // Duration of the trail
            trailRenderer.startWidth = 0.5f;  // Width of the trail at the start
            trailRenderer.endWidth = 0.1f;    // Width of the trail at the end
            // a material to the trail
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            // Set the colour gradient along the trail.
            Gradient gradient = new Gradient();
            Color targetColor = Color.HSVToRGB((float)i / numberOfSphere, 1f, 1f);
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.white, 0f), // (color, normalized position)
                    new GradientColorKey(targetColor, 0.8f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f), // (alpha, normalized position) 
                    new GradientAlphaKey(0f, 1f)
                }
            );
            trailRenderer.colorGradient = gradient;

        }
    }

    void Update()
    {
        /*
        // Loop for N-body gravity
        // How should we design the loop?

        // 00. Initialize the acceleration for each body to zero at the start of each frame
       for (int i = 0; i < numberOfSphere; i++)
        {
            // Something
        }
        // 01. Loop through each body to calculate the gravitational forces acting on it
        for (int i = 0; i < numberOfSphere; i++)
        {
            // Something
            // for ( int j...)
        }
        // 02. Loop through each body to update its velocity and position based on the calculated acceleration
       for (int i = 0; i < numberOfSphere; i++)
        {
            for (int j = 0; j < numberOfSphere; j++)
            {
                bp[i].acceleration = Vector3.zero;
                if (j != i)
                {
                    Vector3 distance = bp[i].body.transform.position - bp[j].body.transform.position;
                    //gravity
                    Vector3 gravity = CalculateGravity(distance, bp[i].mass, bp[j].mass);
                    //acceleration
                    bp[i].acceleration += gravity / bp[i].mass;
                }
            }

            //position
            bp[i].body.transform.position += bp[i].velocity * Time.deltaTime * 30f;
            //velocity
            bp[i].velocity = bp[i].acceleration * Time.deltaTime * 30f;
            */

        //new value of acceleration of every frame
        for (int i = 0; i < numberOfSphere; i++) { 
            bp[i].acceleration = Vector3.zero;
        }

        for (int i = 0; i < numberOfSphere; i++) {
            for (int j = i + 1; j < numberOfSphere; j++)
            {
                Vector3 distance = bp[i].body.transform.position - bp[j].body.transform.position;
                Vector3 gravity = CalculateGravity(distance, bp[i].mass, bp[j].mass);
                bp[i].acceleration -= gravity / bp[i].mass;
                bp[j].acceleration += gravity / bp[j].mass;

                if (distance.magnitude < minimumDistance)
                {
                    bp[i].acceleration = separationForce * gravity / bp[i].mass;
                    bp[i].acceleration = separationForce * gravity / bp[j].mass;
                }

            }
        }

        for (int i = 0; i < numberOfSphere; i++)
        {
            bp[i].velocity += bp[i].acceleration * Time.deltaTime * fasterTime;
            bp[i].body.transform.position += bp[i].velocity * Time.deltaTime * fasterTime;
        }
    }

    // Gravity Fuction to finish
    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)
    {
        Vector3 gravity = Vector3.zero; // note this is also Vector3
                                        // **** Fill in the function below
        gravity = G * m1 * m2 / (distanceVector.sqrMagnitude) * distanceVector.normalized;
        return gravity;
    }
}

