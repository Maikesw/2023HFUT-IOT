import java.io.*;
import java.util.*;
public class Inventory {
    private  String INVENTORY_FILE = "Inventory.txt";
    private  String NEW_INVENTORY_FILE = "NewInventory.txt";
    private  String TRANSACTIONS_FILE = "Transactions.txt";
    private  String SHIPPING_FILE = "Shipping.txt";
    private  String ERRORS_FILE = "Errors.txt";
    private int judge=0;
    private Map<String, InventoryItem> inventory = new HashMap<>();
    private Map<String, ShoppingRecord> shippingRecords = new HashMap<>();
    private List<String> errorRecords = new ArrayList<>();
    public static void main(String[] args) {
        Inventory inventory = new Inventory();
        inventory.loadInventory();
        inventory.processTransactions();
        inventory.saveNewInventory();
        inventory.saveShippingRecords();
        inventory.saveErrorRecords();
    }

    private void loadInventory() {
        File file = new File(INVENTORY_FILE);
        try (BufferedReader br = new BufferedReader(new FileReader(file))) {
            String line;
            while ((line = br.readLine()) != null) {
                String[] parts = line.split("\t");
                if (parts.length < 4) {
                    continue; 
                }
                String itemNumber = parts[0];
                int quantity = Integer.parseInt(parts[1]);
                String supplier = parts[2];
                String description = parts[3];

                InventoryItem item = new InventoryItem();
                item.itemNumber = itemNumber;
                item.quantity = quantity;
                item.supplier = supplier;
                item.description = description;

                inventory.put(itemNumber, item);
            }
        } catch (FileNotFoundException e) {
            System.err.println("Inventory file not found: " + e.getMessage());
        } catch (IOException e) {
            System.err.println("Error reading inventory file: " + e.getMessage());
        } catch (NumberFormatException e) {
            System.err.println("Error parsing quantity in inventory file: " + e.getMessage());
        }
    }

    private void processTransactions() {
        File file = new File(TRANSACTIONS_FILE);
        try (BufferedReader br = new BufferedReader(new FileReader(file))) {
            String line;
            while ((line = br.readLine()) != null) {
                String[] parts = line.split("\t");
                if (parts.length == 0) {
                    continue;
                }
                
                char transactionType = parts[0].charAt(0);
                if(judge==0&&transactionType=='R')
                {
                    Rprocess(parts);
                }
                if(judge==3&&transactionType=='D')
                {
                    Dprocess(parts);
                }
                if(judge==2)
                {
                    switch (transactionType) 
                    {
                    case 'O':{
                        Oprocess(parts);
                        break;
                    }
                    case 'A':
                        Aprocess(parts);
                        break;
                    default:
                        break;
                    }
                }
            }
            if(judge==3)
            {
                return;
            }
            judge+=1;
            processTransactions();
        } 
        catch (FileNotFoundException e) {
            System.err.println("Transactions file not found: " + e.getMessage());
        } catch (IOException e) {
            System.err.println("Error reading transactions file: " + e.getMessage());
        }
    }
    
    private void Oprocess(String[] parts) 
    {
        if (parts.length < 4) 
        {
            return;
        }
        String itemNumber = parts[1];
        int orderQuantity;
        try 
        {
            orderQuantity = Integer.parseInt(parts[2]);
        } catch (NumberFormatException e) 
        {
            return;
        }
    
        InventoryItem item = inventory.get(itemNumber);
        if (item.quantity >= orderQuantity) 
        {
            item.quantity -= orderQuantity; 
            shippingRecords.put(item.itemNumber,new ShoppingRecord(parts[3],item.itemNumber,orderQuantity));
        } 
        else 
        {
            errorRecords.add(item.itemNumber);
        }
    }
    
    private void Rprocess(String[] parts) 
    {
        if (parts.length < 2) 
        {
            return;
        }
        String itemNumber = parts[1];
        int orderQuantity;
        try 
        {
            orderQuantity = Integer.parseInt(parts[2]);
        } catch (NumberFormatException e) 
        {
            return;
        }
        InventoryItem item = inventory.get(itemNumber);
        item.quantity+=orderQuantity;

    }
    
    private void Aprocess(String[] parts) 
    {
        if (parts.length < 4) 
        {
            return;
        }
        InventoryItem item = new InventoryItem();

                item.itemNumber = parts[1];
                item.quantity = 0;
                item.supplier = parts[2];
                item.description = parts[3];
                inventory.put(parts[1], item);
    }
    
    private void Dprocess(String[] parts) 
    {
        if (parts.length < 2) 
        {
            return;
        }

        InventoryItem item = inventory.get(parts[1]);
        if(item.quantity==0)
        {
            inventory.remove(item.itemNumber);
        }
        else
        {
            errorRecords.add(item.itemNumber);
        }
    }

    private void saveNewInventory() {
        try (PrintWriter out = new PrintWriter(new FileWriter(NEW_INVENTORY_FILE))) {
            for (InventoryItem item : inventory.values()) {
                out.println(item.itemNumber + "\t" + item.quantity + "\t" + item.supplier + "\t" + item.description);
            }
        } catch (IOException e) {
            System.err.println("Error writing new inventory file: " + e.getMessage());
        }
    }

    private void saveShippingRecords() {
        try (PrintWriter out = new PrintWriter(new FileWriter(SHIPPING_FILE))) {
            for (ShoppingRecord record : shippingRecords.values()) {
                out.println(record.customer + "\t" + record.itemNumber + "\t" + record.quantity);
            }
        } catch (IOException e) {
            System.err.println("Error writing shipping records file: " + e.getMessage());
        }
    }

    private void saveErrorRecords() {
        try (PrintWriter out = new PrintWriter(new FileWriter(ERRORS_FILE))) {
            for (String errorRecord : errorRecords) {
                out.println(errorRecord);
            }
        } catch (IOException e) {
            System.err.println("Error writing error records file: " + e.getMessage());
        }
    }

    private class InventoryItem 
    {
        String itemNumber;
        int quantity;
        String supplier;
        String description;
    }

    private class ShoppingRecord 
    {
        String customer;
        String itemNumber;
        int quantity;
ShoppingRecord(String cus,String ite,int qua)
{
    customer=cus;
    itemNumber=ite;
    quantity=qua;
}
    }
}











