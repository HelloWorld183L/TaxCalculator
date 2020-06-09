module TaxDomain
    open System

    let parseMoney(input : string) =
        match Decimal.TryParse input with
        | true, sum ->
            if sum >= 0m then Ok sum
            else Error "The total gross income must be greater than 0, please enter the correct input again!"
        | _ -> Error "Invalid number!" 

    let (|Yes|No|InputError|) = function
        | "Y" | "y" -> Yes
        | "N" | "n" -> No
        | _ -> InputError

    let rec getLivesInScotland response errorFunc =
        match response with
            | Yes -> true
            | No -> false
            | InputError ->
                errorFunc()
                getLivesInScotland response errorFunc

    let rec getIsBlindAllowance response errorFunc =
        match response with
            | Yes -> 2500m
            | No -> 0m
            | InputError ->
                errorFunc()
                getIsBlindAllowance response errorFunc

    let getCommonTaxRate totalGrossIncome =
        if totalGrossIncome <= 12500m then 0m
        elif totalGrossIncome <= 50000m then 0.2m
        elif totalGrossIncome <= 150000m then 0.4m
        else 0.45m
    
    let getScotlandTaxRate totalGrossIncome =
        if totalGrossIncome <= 12500m then 0m
        elif totalGrossIncome <= 14585m then 0.19m
        elif totalGrossIncome <= 25158m then 0.2m
        elif totalGrossIncome <= 150000m then 0.41m
        else 0.46m

    let getPersonalAllowance totalGrossIncome allowances =
        let allowanceSum = Seq.sum allowances
        let defaultPersonalAllowance = 12500m + allowanceSum

        if totalGrossIncome <= 100000m then defaultPersonalAllowance
        else
            let evenCount = (totalGrossIncome - 100000m) / 2m
            let lessThanZero = (defaultPersonalAllowance - evenCount) < 0m
            if lessThanZero then 0m
            else evenCount
        
    let rec getMarriageAllowance decision totalGrossIncome errorFunc =
        if totalGrossIncome <= 12500m then 0m
        else
            match decision with
            | Yes -> 1250m
            | No -> 0m
            | InputError ->
                errorFunc()
                getMarriageAllowance decision totalGrossIncome errorFunc