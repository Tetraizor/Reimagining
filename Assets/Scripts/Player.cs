using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBehaviour
{
    [HideInInspector] public static Player instance;
    
    public LayerMask dreamMask;
    public LayerMask dreamButtonMask;
    public LayerMask entityMask;
    public LayerMask entityAndBlockFilter;
    public LayerMask visionMask;

    float horizontalAxis, verticalAxis;
    public float moveSpeed, jumpThrust;

    [HideInInspector] public bool pickerActive;

    public DreamEffector currentEffector;
    private DreamEffector holdingEffector;

    Vector2 bubbleTargetScale;

    public List<GameObject> requested = new List<GameObject>();

    public bool canInput = true;

    GameObject pickerGrid;
    GameObject bubble;
    GameObject visionLine;

    ParticleSystem dust;

    [SerializeField] GameObject jumpDust;
    [SerializeField] GameObject previewTile;

    Animator playerAnimator;
    public GameObject chosenObject;

    [HideInInspector] public bool isRequestOn;
    [HideInInspector] public int requestIndex;

    public void Awake()
    {
        entityRigidbody = gameObject.GetComponent<Rigidbody2D>();
        startGravity = entityRigidbody.gravityScale;
        instance = this;
    }

    public void Start()
    {
        pickerGrid = transform.Find("PickerGrid").gameObject;
        pickerGrid.SetActive(false);
        pickerGrid.transform.parent = null;
        bubble = gameObject.transform.Find("Bubble").gameObject;
        dust = gameObject.transform.Find("Dust").gameObject.GetComponent<ParticleSystem>();
        visionLine = gameObject.transform.Find("VisionLine").gameObject;
        visionLine.SetActive(false);
        playerAnimator = GetComponent<Animator>();
        canInput = true;
    }

    public void Update()
    {
        GetInput();
        if(pickerActive)
        {
            Picker();
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && GroundCheck() && !isClimbing)
        {
            Jump();
            gameObject.GetComponent<AudioManager>().PlayAudio(9);
        }

        if(canInput)
        {
            ClickManager();
        }
        
        bubble.gameObject.GetComponent<SpriteRenderer>().size = Vector2.Lerp(bubble.gameObject.GetComponent<SpriteRenderer>().size, bubbleTargetScale, 10f * Time.deltaTime);



    }

    public void FixedUpdate()
    {
        MovePlayer();
    }

    public void GetInput()
    {
        if(canInput)
        {
            horizontalAxis = Input.GetAxis("Horizontal");
            verticalAxis = Input.GetAxis("Vertical");
        }
        else
        {
            horizontalAxis = 0;
            verticalAxis = 0;
        }
    }

    public void MovePlayer()
    {
        playerAnimator.SetFloat("HorizontalAxis", Mathf.Abs(horizontalAxis));
        playerAnimator.SetFloat("MovementAxis", Mathf.Abs(horizontalAxis) + Mathf.Abs(verticalAxis));
        playerAnimator.SetBool("IsGrounded", GroundCheck());
        playerAnimator.SetBool("IsClimbing", isClimbing);

        transform.position += (Vector3)(Vector2.right * horizontalAxis * Time.fixedDeltaTime * moveSpeed);

        if(horizontalAxis > 0.2f)
        {
            transform.localScale = new Vector3(1, orientation, 1);
        }
        else if(horizontalAxis < -0.2f)
        {
            transform.localScale = new Vector3(-1, orientation, 1);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, orientation, 1);
        }

        if(!isClimbing && !isFloating && GroundCheck() && Mathf.Abs(horizontalAxis) > 0.2f)
        {
            if(!dust.isEmitting)
            {
                dust.Play();
            }
        }
        else
        {
            if(dust.isEmitting)
            {
                dust.Stop();
            }
        }

        if(isClimbing)
        {
            transform.position += (Vector3)(transform.up * verticalAxis * Time.fixedDeltaTime * moveSpeed);
            entityRigidbody.gravityScale = 0;
        }
        else if(isFloating)
        {
            entityRigidbody.gravityScale = 0.5f * startGravity * orientation;
        }
        else
        {
            entityRigidbody.gravityScale = startGravity * orientation;
        }

    }

    public void Jump()
    {   
        GameObject particleSystem = Instantiate(jumpDust, transform.position, jumpDust.transform.rotation);
        particleSystem.transform.parent = null;

        particleSystem.GetComponent<ParticleSystem>().Play();

        Destroy(particleSystem, 1);

        if(isFloating)
        {
            entityRigidbody.AddForce(transform.up * orientation * jumpThrust / 1.2f, ForceMode2D.Impulse);
        }
        else
        {
            entityRigidbody.AddForce(transform.up * orientation * jumpThrust, ForceMode2D.Impulse);
        }

        
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.transform.tag == "Platform")
        {
            ObjectProperties triggerProperty = trigger.gameObject.GetComponent<ObjectProperties>();

            if(triggerProperty.canClimb)
            {
                isClimbing = true;
                entityRigidbody.velocity = Vector2.zero;
            }

            if(triggerProperty.canSwim)
            {
                isFloating = true;
                if(orientation * entityRigidbody.velocity.y < 0)
                {
                    entityRigidbody.velocity = new Vector2(entityRigidbody.velocity.x, 0);
                }
            }

            if(triggerProperty.collisionKills)
            {
                KillPlayer();
            }
        }
        else if(trigger.tag == "KillArea")
        {
            KillPlayer();
        }
    }

    void OnTriggerStay2D(Collider2D trigger)
    {
        if(trigger.transform.tag == "Platform")
        {
            ObjectProperties triggerProperty = trigger.gameObject.GetComponent<ObjectProperties>();

            if(triggerProperty.canClimb)
            {
                isClimbing = true;
            }

            if(triggerProperty.canSwim)
            {
                isFloating = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if(trigger.transform.tag == "Platform")
        {
            ObjectProperties triggerProperty = trigger.gameObject.GetComponent<ObjectProperties>();

            if(triggerProperty.canClimb)
            {
                isClimbing = false;
            }

            if(triggerProperty.canSwim)
            {
                isFloating = false;
            }
        }
    }

    public void KillPlayer()
    {
        if(!isDead)
        {
            print("Died.");
            canInput = false;
            playerAnimator.SetTrigger("Dead");
            DeactivateBlockPicker();
            GameManager.instance.TextPanelIn("Hedefe ulaşamadın. Tekrar denemek icin 'R' tuşuna bas.");
            isDead = true;
        }
    }

    public void ClickManager()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(pickerActive)
            {
                if(chosenObject != null)
                {
                    if(requestIndex == 3) // EFFECTED DEĞİŞTİR, ENTITY VEYA BLOK OLABİLİR
                    {
                        currentEffector.effectedBlock = chosenObject;

                        GameObject preview = Instantiate(previewTile, chosenObject.transform.position, previewTile.transform.rotation); // DO THIS FOR PLAYER

                        if(currentEffector.effectedRenderer.transform.childCount > 0)
                        {
                            Destroy(currentEffector.effectedRenderer.transform.GetChild(0).gameObject);
                        }
                        if(chosenObject.GetComponent<Player>())
                        {
                            preview.GetComponent<SpriteRenderer>().sprite = chosenObject.transform.Find("Renderer").GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                        {
                            preview.GetComponent<SpriteRenderer>().sprite = chosenObject.GetComponent<SpriteRenderer>().sprite;
                        }
                        
                        preview.GetComponent<PreviewTile>().target = Vector3.zero;
                        preview.transform.parent = currentEffector.effectedRenderer.transform;

                        DeactivateBlockPicker();
                        requestIndex = -1;

                        currentEffector.effectedRenderer.GetComponent<Animation>().Stop();
                    }
                    if(requestIndex == 1) // EFFECTED DEĞİŞTİR, SADECE BLOK
                    {
                        currentEffector.effectedBlock = chosenObject;

                        GameObject preview = Instantiate(previewTile, chosenObject.transform.position, previewTile.transform.rotation);
                        if(currentEffector.effectedRenderer.transform.childCount > 0)
                        {
                            Destroy(currentEffector.effectedRenderer.transform.GetChild(0).gameObject);
                        }
                        preview.GetComponent<SpriteRenderer>().sprite = chosenObject.GetComponent<SpriteRenderer>().sprite;
                        preview.GetComponent<PreviewTile>().target = Vector3.zero;
                        preview.transform.parent = currentEffector.effectedRenderer.transform;

                        DeactivateBlockPicker();
                        requestIndex = -1;

                        currentEffector.effectedRenderer.GetComponent<Animation>().Stop();
                    }
                    if(requestIndex == 2)
                    {
                        currentEffector.effectorBlock = chosenObject;

                        GameObject preview = Instantiate(previewTile, chosenObject.transform.position, previewTile.transform.rotation);
                        if(currentEffector.effectorRenderer.transform.childCount > 0)
                        {
                            Destroy(currentEffector.effectorRenderer.transform.GetChild(0).gameObject);
                        }
                        preview.GetComponent<SpriteRenderer>().sprite = chosenObject.GetComponent<SpriteRenderer>().sprite;
                        preview.GetComponent<PreviewTile>().target = Vector3.zero;
                        preview.transform.parent = currentEffector.effectorRenderer.transform;

                        DeactivateBlockPicker();
                        requestIndex = -1;

                        currentEffector.effectorRenderer.GetComponent<Animation>().Stop();
                    }
                }
            }

            RaycastHit2D raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, dreamMask);
            if(raycast.transform != null)
            {
                if(raycast.transform.tag == "DreamEffector")
                {
                    holdingEffector = raycast.transform.gameObject.GetComponent<DreamEffector>();
                }
            }

            // HANDLE DREAM BUTTONS
            RaycastHit2D raycastButton = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, dreamButtonMask);
            if(currentEffector != null)
            {
                if(raycastButton.transform != null)
                {
                    if(raycastButton.transform.parent == currentEffector.transform)
                    {
                        currentEffector.ButtonPress(raycastButton.transform.GetComponent<DreamButton>().index);
                        gameObject.GetComponent<AudioManager>().PlayAudio(10);
                    }
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(holdingEffector != null)
            {
                if((holdingEffector.transform.position - transform.position).magnitude < 5)
                {
                    if(currentEffector)
                    {
                        currentEffector.target = (Vector2)Camera.main.transform.position + new Vector2(Random.Range(-(Camera.main.orthographicSize * Camera.main.aspect)/2, (Camera.main.orthographicSize * Camera.main.aspect)/2), Random.Range(-(Camera.main.orthographicSize)/2, (Camera.main.orthographicSize)/2));
                    }
                    currentEffector = holdingEffector;
                }
                else
                {
                    holdingEffector.target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                holdingEffector = null;
            }
        }

        if(holdingEffector)
        {
            holdingEffector.target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if((holdingEffector.transform.position - transform.position).magnitude < 5)
            {
                Vector2 distanceVector = (holdingEffector.transform.position - (transform.position + (Vector3)(Vector2.up / 2)));
                bubbleTargetScale = new Vector2(distanceVector.magnitude, 0.25f);
                bubble.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(distanceVector.y, distanceVector.x));
                bubble.transform.position = transform.position + (holdingEffector.transform.position - transform.position)/2;
            }
            else
            {
                bubbleTargetScale = new Vector3(0, 0, 1);
            }
        }

        if(currentEffector)
        {
            currentEffector.target = (Vector2)transform.position + new Vector2(-3, 3);

            Vector2 distanceVector = (currentEffector.transform.position - (transform.position + (Vector3)(Vector2.up / 2) * orientation));
            bubbleTargetScale = new Vector2(distanceVector.magnitude, 0.25f);
            bubble.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(distanceVector.y, distanceVector.x));
            bubble.transform.position = transform.position + (currentEffector.transform.position - transform.position)/2;
            
        }

        if(holdingEffector == null && currentEffector == null)
        {
            bubbleTargetScale = new Vector3(0, 0.25f, 0);
        }
        
    }

    public void Picker()
    {
        RaycastHit2D[] hit = Physics2D.LinecastAll((Vector2)(transform.position + (new Vector3(0, 1f, 0) * orientation)), Camera.main.ScreenToWorldPoint(Input.mousePosition), visionMask);
        if(hit.Length > 0)
        {
            foreach(RaycastHit2D raycastHit in hit)
            {
                if(raycastHit.transform == transform)
                {
                    RaycastHit2D raycastDirectHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, visionMask);
                    if(raycastDirectHit.transform)
                    {
                        if(raycastDirectHit == transform)
                        {
                            chosenObject = gameObject;
                            break;
                        }
                    }
                }
            }

            foreach(RaycastHit2D raycastHit in hit)
            {
                if(raycastHit.transform.GetComponent<Enemy>())
                {
                    RaycastHit2D raycastDirectHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, visionMask);
                    if(raycastDirectHit.transform)
                    {
                        if(raycastDirectHit.transform == raycastHit.transform)
                        {
                            chosenObject = raycastDirectHit.transform.gameObject;
                        }
                    }
                    else
                    {
                        chosenObject = null;
                        break;
                    }
                }
                else if(raycastHit.transform.GetComponent<ObjectProperties>())
                {
                    RaycastHit2D raycastDirectHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, visionMask);
                    
                    if(raycastHit.transform == raycastDirectHit.transform)
                    {
                        chosenObject = raycastDirectHit.transform.gameObject;
                    }
                    else
                    {
                        chosenObject = null;
                        if(!raycastHit.transform.GetComponent<ObjectProperties>().isTransparent)
                        {
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            RaycastHit2D raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, visionMask);
            if(raycast.transform)
            {
                chosenObject = raycast.transform.gameObject;
            }
            else
            {
                chosenObject = null;
            }
        }

        Vector2 delta = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position + new Vector3(0, 1f, 0) * orientation));
        visionLine.GetComponent<SpriteRenderer>().size = new Vector2(delta.magnitude, 0.125f);
        visionLine.transform.rotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x)));
        visionLine.transform.position = transform.position + new Vector3(0, 1, 0) * orientation + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position + new Vector3(0, 1, 0) * orientation))/2;

        if(chosenObject)
        {
            if(chosenObject.GetComponent<Enemy>() || chosenObject.GetComponent<Player>())
            {
                pickerGrid.transform.position = chosenObject.transform.position - new Vector3(0.5f, 0, 0);
            }
            else
            {
                pickerGrid.transform.position = chosenObject.GetComponent<ObjectProperties>().position;
            }
        }
        else
        {
            pickerGrid.transform.position = new Vector2(-128, -128);
        }
    }
    
    public void ActivateBlockPicker()
    {
        pickerGrid.SetActive(true);
        visionLine.SetActive(true);
        pickerActive = true;
    }

    public void DeactivateBlockPicker()
    {
        pickerGrid.SetActive(false);
        visionLine.SetActive(false);
        pickerActive = false;
        gameObject.GetComponent<AudioManager>().PlayAudio(8);
        bubbleTargetScale = Vector2.zero;
    }

    public Vector2Int FloatVectorToInt(Vector2 _floatVector)
    {
        return new Vector2Int(Mathf.RoundToInt(_floatVector.x - 0.5f), Mathf.RoundToInt(_floatVector.y - 0.5f));
    }
}
