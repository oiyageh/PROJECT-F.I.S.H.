using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class LargeMonsterAI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Vision")]
    public float sightDistance = 25f;

    [Tooltip("How long monster remembers player after losing sight")]
    public float memoryTime = 3f;

    [Tooltip("EyePosition")]
    public Transform eyes;

    [Header("Wander")]
    public float wanderRadius = 20f;
    public float wanderDelay = 5f;

    [Header("Movement")]
    public float wanderSpeed = 3.5f;
    public float chaseSpeed = 6f;

    [Header("Monster Sounds")]
    public AudioClip[] ambientSounds;

    [Tooltip("Minimum time before another sound")]
    public float minSoundDelay = 8f;

    [Tooltip("Maximum time before another sound")]
    public float maxSoundDelay = 20f;

    [Range(0f, 1f)]
    public float soundVolume = 1f;

    [Header("Lose Screen")]
    public string loseSceneName = "LoseScene";

    private NavMeshAgent agent;
    private AudioSource audioSource;

    private bool seesPlayer;

    private float wanderTimer;
    private float searchTimer;
    private float lostSightTimer;

    private float soundTimer;
    private float nextSoundTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        FindPlayer();

        Wander();

        SetNextSoundTime();

        // Audio settings
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
    }

    void Update()
    {
        // Re-find player after scene changes
        if (player == null)
        {
            searchTimer += Time.deltaTime;

            if (searchTimer >= 1f)
            {
                FindPlayer();
                searchTimer = 0;
            }

            WanderLogic();
            PlayAmbientSounds();
            return;
        }

        DetectPlayer();

        if (seesPlayer)
        {
            ChasePlayer();
        }
        else
        {
            WanderLogic();
        }

        PlayAmbientSounds();

        // Rotate toward movement direction
        if (agent.velocity.magnitude > 0.1f)
        {
            Vector3 dir = agent.velocity.normalized;
            dir.y = 0;

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * 5f);
        }
    }

    void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
        }
    }

    void DetectPlayer()
    {
        float distance =
            Vector3.Distance(transform.position, player.position);

        // Too far away
        if (distance > sightDistance)
        {
            LoseSight();
            return;
        }

        Vector3 dir =
            (player.position - transform.position).normalized;

        Vector3 origin =
            eyes != null ?
            eyes.position :
            transform.position + Vector3.up;

        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                seesPlayer = true;
                lostSightTimer = memoryTime;
                return;
            }
        }

        LoseSight();
    }

    void LoseSight()
    {
        if (seesPlayer)
        {
            lostSightTimer -= Time.deltaTime;

            if (lostSightTimer <= 0)
            {
                seesPlayer = false;
            }
        }
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void WanderLogic()
    {
        agent.speed = wanderSpeed;

        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderDelay || !agent.hasPath)
        {
            Wander();
            wanderTimer = 0;
        }
    }

    void Wander()
    {
        Vector3 randomDir =
            Random.insideUnitSphere * wanderRadius;

        randomDir += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            randomDir,
            out hit,
            wanderRadius,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void PlayAmbientSounds()
    {
        if (ambientSounds.Length == 0)
            return;

        soundTimer += Time.deltaTime;

        if (soundTimer >= nextSoundTime)
        {
            if (!audioSource.isPlaying)
            {
                AudioClip clip =
                    ambientSounds[
                        Random.Range(0, ambientSounds.Length)];

                audioSource.PlayOneShot(clip, soundVolume);
            }

            soundTimer = 0;
            SetNextSoundTime();
        }
    }

    void SetNextSoundTime()
    {
        nextSoundTime =
            Random.Range(minSoundDelay, maxSoundDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(loseSceneName);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin =
            eyes != null ?
            eyes.position :
            transform.position + Vector3.up;

        // Vision range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightDistance);

        // Look direction
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(origin, transform.forward * sightDistance);

        // Player line
        if (player != null)
        {
            Gizmos.color = seesPlayer ? Color.green : Color.yellow;
            Gizmos.DrawLine(origin, player.position);
        }

        // Destination line
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }
}