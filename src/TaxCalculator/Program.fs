open System
open TaxDomain

module CLIModule =
    let inputBlindAllowance =
        Console.Write("Are you blind? (Y/N)")
        Console.ReadLine()
        |> getIsBlindAllowance

    let inputMarriageAllowance totalGrossIncome =
        Console.Write("Has your partner transfered marriage allowance to you? (Y/N)")
        let response = Console.ReadLine()
        getMarriageAllowance response totalGrossIncome

    let inputLivesInScotland =
        Console.Write("Do you live in Scotland? (Y/N)")
        Console.ReadLine()
        |> getLivesInScotland

    [<EntryPoint>]
    let main argv =
        Console.Write("Enter your total gross income per year: ")
        let totalGrossIncome = Console.ReadLine() |> parseMoney
        match totalGrossIncome with
        | Error e ->
            Console.WriteLine e
            0
        | Ok totalGrossIncome ->
            let allowances = [inputBlindAllowance; inputMarriageAllowance totalGrossIncome]
            let personalAllowance = getPersonalAllowance totalGrossIncome allowances
            let taxableIncome = totalGrossIncome - personalAllowance
            let livesInScotland = inputLivesInScotland
            let taxRate =
                if livesInScotland then getScotlandTaxRate totalGrossIncome
                else getCommonTaxRate totalGrossIncome
            let incomeTax = taxableIncome * taxRate
            Console.WriteLine("Your income tax is: " + incomeTax.ToString())
            0