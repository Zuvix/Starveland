using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : CellObject
{
    public float movementSpeed=2f;
    public float gatheringSpeed = 1.5f;
    public Resource itemInHand=null;
    bool isBusy = false;

    // Update is called once per frame
    public IEnumerator MoveUnitToNextPosition(Cell currentCell,Cell nextCell)
    {
        //Set unit to be busy
        isBusy = true;
        //check if nextCell is empty
        if (nextCell.GetCellObject() == null)
        {

            //set cell to be used by unit, free the old cell
            nextCell.SetCellObject(this.gameObject);
            currentCell.SetCellObject(null);

            //calculate distance and movement vector
            float distance;
            distance = Vector3.Distance(transform.position, nextCell.position);
            Vector3 movementVector = nextCell.position - transform.position;
            movementVector = movementVector.normalized;

            //turn on animations
            ScalingAnim(true);
            RotationAnim(true);

            //Flip unit towards position
            if (nextCell.position.x > transform.position.x)
            {
                Flip("right");
            }
            else if (nextCell.position.x < transform.position.x)
            {
                Flip("left");
            }

            //initiate ingame movement process
            while (distance > 0.5f)
            {
                transform.position += movementVector * movementSpeed * Time.deltaTime;
                yield return new WaitForFixedUpdate();
                distance = Vector3.Distance(transform.position, nextCell.position);
            }
            //turn off animations
            ScalingAnim(false);
            RotationAnim(false);

            //center units position to cells position
            transform.position = nextCell.position;
        }
        isBusy = false;
    }
    public IEnumerator GatherResource(ResourceSource target)
    {
        if (itemInHand == null)
        {
            Debug.Log("Preparing the axe");
            yield return new WaitForSeconds(gatheringSpeed);
            Debug.Log("Gathering object");
            itemInHand = target.Gather();
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            Debug.Log("my hand is full");
        }
    }
    /*
    public Resource StoreResource()
    {
        if (itemInHand != null)
        {
            Debug.Log("Storing resource with name:" + itemInHand.name);
            Resource storedResource=itemInHand;
            itemInHand = null;
            return storedResource;
        }
    }
    */
}
